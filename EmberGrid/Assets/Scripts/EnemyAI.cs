using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI
{
    private Unit unit;
    private GridBuilder gridBuilder => GameManager.Instance.GridBuilder;
    private Targeter targeter => gridBuilder.Targeter;

    // ---- Scoring Weights (tune these) ----
    private const float DAMAGE_WEIGHT = 1.0f;             // how much raw damage matters
    private const float DIST_TO_HIT_MIN_WEIGHT = 3.0f;    // keep max distance from the closest *hit* hero (post-attack safety)
    private const float DIST_TO_HIT_AVG_WEIGHT = 0.5f;    // tie-break among equal min-distance options
    private const float NON_TARGET_NEAREST_WEIGHT = 0.25f;// small bias to stay away from the nearest *non-hit* hero
    private const float NO_HIT_APPROACH_WEIGHT = 1.0f;    // on no-hit turns, prefer to CLOSE distance (approach), not kite
    private const float EPS_TIE = 0.001f;                 // tiny nudge to break ties toward safer choices

    public EnemyAI(Unit givenOwner)
    {
        unit = givenOwner;
    }

    /// <summary>
    /// Drives one enemy’s turn: picks the best stand+execute combo, moves, then attacks.
    /// Attacks are ALWAYS preferred over non-attack moves.
    /// </summary>
    public IEnumerator TakeTurn(Action onComplete)
    {
        // 1) Gather AP and available actions
        int remainingAP = unit.ActionHandler.CanTakeAction ? 1 : 0;
        var actions = unit.WeaponHandler.GetAvailableActions(remainingAP);

        // 2) Gather all player units
        var heroes = GameManager.Instance.PlayerManager.Team.Units;

        // 3) Cache movement stand positions
        var originTile = unit.Movement.CurrentTile;
        unit.Movement.SetMovementMode();
        var standTiles = unit.Movement.CurrentReach.ToList();
        standTiles.Add(originTile);

        // Keep separate bests so any hit beats any non-hit
        AIChoice bestAttack = new AIChoice { Score = float.NegativeInfinity, hit = false };
        AIChoice bestMove = new AIChoice { Score = float.NegativeInfinity, hit = false };

        // 4) Two-stage search: stand tile -> execute tile
        foreach (var action in actions)
        {
            foreach (var standTile in standTiles) // every tile the enemy can stand on
            {
                // Per-standing-tile distance cache (standing -> each hero)
                var distCache = new Dictionary<Unit, int>();
                foreach (var h in heroes)
                {
                    var heroTile = h?.Movement?.CurrentTile;
                    if (heroTile != null)
                        distCache[h] = Pathfinder.GetDistanceOfTiles(standTile.Pos, heroTile.Pos);
                }

                // Find all tiles from which the unit could execute this action
                var execTiles = gridBuilder.GetTilesInReach(standTile, action.Range);

                // NOTE: even if we don't hit, we still evaluate to decide the best *approach* stand tile
                foreach (var execTile in execTiles) // every tile the enemy can attack from
                {
                    foreach (Direction dir in Enum.GetValues(typeof(Direction))) // every possible rotation
                    {
                        var hitbox = targeter.GetHitbox(action, dir, execTile.Pos);

                        // Skip combos hitting any allied (enemy) unit, including itself
                        if (hitbox.Any(t => ReferenceEquals(t, standTile) || (t.SubscribedUnit != null && t.SubscribedUnit is Enemy)))
                            continue;

                        float score = Score(
                            action,
                            hitbox,
                            heroes,
                            execTile,
                            standTile,
                            distCache,
                            out bool willHit);

                        if (willHit)
                        {
                            if (score > bestAttack.Score)
                            {
                                bestAttack.Action = action;
                                bestAttack.Direction = dir;
                                bestAttack.MoveTo = standTile;
                                bestAttack.executeAt = execTile;
                                bestAttack.Score = score;
                                bestAttack.hit = true;
                            }
                        }
                        else
                        {
                            // Track the best non-attack move (approach)
                            if (score > bestMove.Score)
                            {
                                bestMove.Action = action;
                                bestMove.Direction = dir;
                                bestMove.MoveTo = standTile;
                                bestMove.executeAt = execTile;
                                bestMove.Score = score;
                                bestMove.hit = false;
                            }
                        }
                    }
                }
            }
        }

        // 5) Prefer an attack if any exist; otherwise fall back to best move (approach) or wait
        AIChoice chosen = bestAttack.hit ? bestAttack : bestMove;

        if (chosen.Action != null && chosen.MoveTo != null)
        {
            Debug.Log($"{chosen.MoveTo.Pos} standing tile, execute action @{chosen.executeAt.Pos}, facing {chosen.Direction} | hit={chosen.hit} | score={chosen.Score}");

            // Pathfind and move to the chosen stand tile
            var path = gridBuilder.Pathfinder.FindPathToDest(
                originTile,
                chosen.MoveTo,
                gridBuilder.WalkableDictionary);

            yield return unit.StartCoroutine(unit.Movement.MoveAlongPath(path));

            // Subscribe unit to the stand tile
            chosen.MoveTo.SubUnit(unit);

            if (bestAttack.hit) // only execute if we're actually attacking
            {
                unit.WeaponHandler.ExecuteActionAt(
                    chosen.Action,
                    chosen.Direction,
                    chosen.executeAt);
            }
            else
            {
                // Best we found was a setup/approach move
                unit.ActionHandler.TakeWaitAction();
            }

            yield return new WaitForSeconds(0.2f);
        }

        // 6) Done
        onComplete?.Invoke();
    }

    /// <summary>
    /// Score = raw effect value (damage/heal) + (if hitting) distance safety from targets.
    /// Safety is computed from the STANDING tile to hero units (not standing↔exec).
    /// For multi-target AoE, maximize the minimum distance to any hit hero.
    /// If NOT hitting, this returns an "approach" score that prefers CLOSER distance to heroes.
    /// </summary>
    private float Score(
        UnitAction action,
        TileSD[] hitbox,
        IEnumerable<Unit> heroes,
        TileSD execTile,
        TileSD standingTile,
        Dictionary<Unit, int> distCache,
        out bool willHit)
    {
        willHit = false;
        float damageScore = 0f;

        // Collect which heroes would be hit
        var hitHeroes = new List<Unit>();

        foreach (var tile in hitbox)
        {
            var target = tile.SubscribedUnit;
            if (target == null) continue;

            foreach (var eff in action.Effects)
            {
                if (eff is DamageAE dmg && target is Hero)
                {
                    damageScore += dmg.GetDamageFromUnit(unit);
                    willHit = true;
                    if (!hitHeroes.Contains(target)) hitHeroes.Add(target);
                }
                else if (eff is HealAE heal && target is Enemy enemy)
                {
                    damageScore += heal.GetHealFromUnit(unit, enemy);
                    willHit = true; // impactful non-damage action
                }
            }
        }

        // Helper: distance from standing tile to a unit (from cache, fallback to compute)
        int DistTo(Unit u)
        {
            if (u == null) return int.MaxValue;
            if (distCache != null && distCache.TryGetValue(u, out var d)) return d;
            var tile = u.Movement?.CurrentTile;
            if (tile == null) return int.MaxValue;
            return Pathfinder.GetDistanceOfTiles(standingTile.Pos, tile.Pos);
        }

        if (willHit && hitHeroes.Count > 0)
        {
            // ---- Post-attack safety: prefer farthest stance from the enemies we hit ----
            int minToHit = int.MaxValue;
            int sumToHit = 0;
            foreach (var h in hitHeroes)
            {
                int d = DistTo(h);
                if (d < minToHit) minToHit = d;
                sumToHit += d;
            }
            float avgToHit = (float)sumToHit / hitHeroes.Count;

            // Also consider the nearest non-hit hero (avoid being close to others)
            int minNonHit = int.MaxValue;
            foreach (var h in heroes)
            {
                if (h == null || hitHeroes.Contains(h)) continue;
                int d = DistTo(h);
                if (d < minNonHit) minNonHit = d;
            }
            if (minNonHit == int.MaxValue) minNonHit = 0;

            float distScore = 0f;
            distScore += minToHit * DIST_TO_HIT_MIN_WEIGHT;
            distScore += avgToHit * DIST_TO_HIT_AVG_WEIGHT;
            distScore += minNonHit * NON_TARGET_NEAREST_WEIGHT;
            distScore += EPS_TIE; // tie nudge

            return damageScore * DAMAGE_WEIGHT + distScore;
        }
        else
        {
            // ---- No hit this turn: APPROACH instead of kiting ----
            // Prefer standing tiles that minimize distance to the nearest hero (so we close in to attack soon).
            int nearestAny = int.MaxValue;
            foreach (var h in heroes)
            {
                if (h == null) continue;
                int d = DistTo(h);
                if (d < nearestAny) nearestAny = d;
            }
            if (nearestAny == int.MaxValue) nearestAny = 0;

            // Smaller distance => larger score (approach)
            float approachScore = (-nearestAny) * NO_HIT_APPROACH_WEIGHT;

            // You could add LOS desirability or “within cast range next turn” heuristics here if available.

            return approachScore; // no damage since we're not hitting
        }
    }

    private class AIChoice
    {
        public UnitAction Action;
        public Direction Direction;
        public TileSD MoveTo;
        public TileSD executeAt;
        public float Score;
        public bool hit;
    }
}

