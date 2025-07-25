using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI
{
    private Unit unit;
    private GridBuilder gridBuilder => GameManager.Instance.GridBuilder;
    private Targeter targeter => GameManager.Instance.GridBuilder.Targeter;

    // Tune how much ranged enemies prefer maximum distance
    private const float DISTANCE_WEIGHT = 1f;

    public EnemyAI(Unit givenOwner)
    {
        unit = givenOwner;
    }

    /// <summary>
    /// Drives one enemy’s turn: picks the best move+action, moves, then executes it.
    /// </summary>
    public IEnumerator TakeTurn(Action onComplete)
    {
        // 1) Gather AP and available actions
        int remainingAP = unit.ActionHandler.CanTakeAction ? 1 : 0;
        var actions = unit.WeaponHandler.GetAvailableActions(remainingAP);

        // 2) Gather all player units
        var heroes = GameManager.Instance.PlayerManager.Team.Units.ToList();

        // 3) Precompute reachable move tiles and evaluate
        var originTile = unit.Movement.CurrentTile;
        var reachable = gridBuilder.GetTilesInReach(originTile, unit.Movement.Speed);

        AIChoice best = new AIChoice { Score = float.MinValue };

        foreach (var action in actions)
        {
            // Determine max offset for this action’s pattern
            int maxOffset = action.Targets
                                  .Select(v => Math.Max(Math.Abs(v.x), Math.Abs(v.y)))
                                  .DefaultIfEmpty(1)
                                  .Max();

            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                foreach (var moveTile in reachable)
                {
                    // Which tiles would this hit?
                    var hitbox = targeter.GetHitbox(action, dir, moveTile.Pos);

                    // Skip combos that would hit allied enemies
                    if (hitbox.Any(t => t.SubscribedUnit != null && t.SubscribedUnit is Enemy))
                        continue;

                    float score = Score(action, hitbox, heroes, moveTile, maxOffset);

                    if (score > best.Score)
                    {
                        best.Action = action;
                        best.Direction = dir;
                        best.MoveTo = moveTile;
                        best.Score = score;
                    }
                }
            }
        }

        // 4) Execute best choice
        if (best.Action != null)
        {
            // Pathfind
            var path = gridBuilder.Pathfinder.FindPathToDest(
                originTile,
                best.MoveTo,
                gridBuilder.WalkableDictionary);

            // Move along path
            yield return unit.StartCoroutine(
                unit.Movement.MoveAlongPath(path));

            // Attack from chosen tile & direction
            unit.WeaponHandler.ExecuteActionAt(
                best.Action,
                best.Direction,
                best.MoveTo);

            yield return new WaitForSeconds(0.2f);
        }

        // 5) Done
        onComplete?.Invoke();
    }

    /// <summary>
    /// Scores action effects plus a distance bonus for ranged attacks.
    /// </summary>
    private float Score(
        UnitAction action,
        TileSD[] tiles,
        List<Unit> heroes,
        TileSD moveTile,
        int maxOffset)
    {
        float total = 0f;

        // Base scoring: damage, heals, buffs
        foreach (var tile in tiles)
        {
            var target = tile.SubscribedUnit;
            if (target == null) continue;

            foreach (var eff in action.Effects)
            {
                switch (eff)
                {
                    case DamageAE dmg:
                        total += dmg.GetDamageFromUnit(unit);
                        break;
                        // add additional cases for healing or buffs as needed
                }
            }
        }

        // Add distance bonus for ranged if any real target hit
        if (maxOffset > 1 && tiles.Any(t => t.SubscribedUnit != null))
        {
            float minDist = float.MaxValue;
            foreach (var tile in tiles)
            {
                if (tile.SubscribedUnit == null) continue;
                float d = Pathfinder.GetDistanceOfTiles(
                    moveTile.Pos,
                    tile.Pos);
                minDist = Mathf.Min(minDist, d);
            }
            total += minDist * DISTANCE_WEIGHT;
        }

        return total;
    }

    private class AIChoice
    {
        public UnitAction Action;
        public Direction Direction;
        public TileSD MoveTo;
        public float Score;
    }
}
