using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyAI
{
    private Unit unit;
    private GridBuilder gridBuilder => GameManager.Instance.GridBuilder;
    private Targeter targeter => gridBuilder.Targeter;

    // Weight for encouraging ranged units to stay at max distance
    private const float DISTANCE_WEIGHT = 1f;

    public EnemyAI(Unit givenOwner)
    {
        unit = givenOwner;
    }

    /// <summary>
    /// Drives one enemy’s turn: picks the best stand+execute combo, moves, then attacks.
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

        AIChoice best = new AIChoice { Score = 0 };

        // 4) Two-stage search: stand tile -> execute tile
        foreach (var action in actions)
        {
            foreach (var standTile in standTiles)//every tile the enemy can stand on
            {
                // Find all tiles from which the unit could execute this action
                var execTiles = gridBuilder.GetTilesInReach(standTile, action.Range);

                foreach (var execTile in execTiles)//every tile the enemy can attack from
                {
                    foreach (Direction dir in Enum.GetValues(typeof(Direction)))//every possible rotation the enemy can attack with 
                    {
                        var hitbox = targeter.GetHitbox(action, dir, execTile.Pos);

                        // Skip combos hitting any allied (enemy) unit, including itself
                        if (hitbox.Any(t => ReferenceEquals(t, standTile) || (t.SubscribedUnit != null && t.SubscribedUnit is Enemy)))
                        {
                            continue;
                        }

                        float score = Score(action, hitbox, heroes, execTile, standTile,out bool hit);
                        if (score > best.Score)
                        {
                            best.Action = action;
                            best.Direction = dir;
                            best.MoveTo = standTile;
                            best.executeAt = execTile;
                            best.Score = score;
                            best.hit = hit;
                        }
                    }
                }
            }
        }

        // 5) Execute the best combo
        if (best.Action != null)
        {
            Debug.Log($"{best.MoveTo.Pos} standing tile, execute action @{best.executeAt.Pos}, facing {best.Direction} with a total score of {best.Score}");

            // Pathfind and move to the chosen stand tile
            var path = gridBuilder.Pathfinder.FindPathToDest(
                originTile,
                best.MoveTo,
                gridBuilder.WalkableDictionary);

            yield return unit.StartCoroutine(
                unit.Movement.MoveAlongPath(path));

            // Subscribe unit to the stand tile
            best.MoveTo.SubUnit(unit);

            if (best.hit)
            {
                // Execute the action from the chosen execute tile and direction
                unit.WeaponHandler.ExecuteActionAt(
                    best.Action,
                    best.Direction,
                    best.executeAt);
            }
            else
            {
                unit.ActionHandler.TakeWaitAction();
            }


            yield return new WaitForSeconds(0.2f);
        }

        // 6) Done
        onComplete?.Invoke();
    }

    /// <summary>
    /// Scores action effects plus a distance bonus for ranged attacks.
    /// </summary>
    private float Score(UnitAction action, TileSD[] hitbox, Unit[] heroes, TileSD execTile, TileSD standingTile, out bool hit)
    {
        float total = 0f;
        hit = false;
        // Base scoring: damage, heals, buffs (here, only damage)
        foreach (var tile in hitbox)
        {
            var target = tile.SubscribedUnit;
            if (target == null) continue;

            foreach (var eff in action.Effects)
            {
                if (eff is DamageAE dmg && target is Hero)
                {
                    total += dmg.GetDamageFromUnit(unit);
                    hit = true;
                }

                else if (eff is HealAE heal && target is Enemy enemy)
                {
                    total += heal.GetHealFromUnit(unit, enemy);
                    hit = true;
                }
                // add cases for HealAE, BuffAE, etc.
            }
        }

        float d = Pathfinder.GetDistanceOfTiles(standingTile.Pos, execTile.Pos);
        float maxDist;

        if (hit)
        {
            maxDist = float.MinValue;
            maxDist = Mathf.Max(maxDist, d);
        }
        else
        {
            maxDist = float.MaxValue;
            maxDist = Mathf.Min(maxDist, d);
        }

        total += maxDist * DISTANCE_WEIGHT;


        return total;
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

