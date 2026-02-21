using System.Linq;
using UnityEngine;

[System.Serializable]
public class UnitTeam
{
    [SerializeField] private Unit[] units;

    public Unit[] Units { get => units; }

    public void Setup()
    {
        foreach (var unit in units)
        {
            unit.OnTurnEnded.AddListener(CheckPhaseEnded);
        }
        PositionUnits();
        UnLockUnits();
    }


    private void PositionUnits()
    {
        var walkable = GameManager.Instance.GridBuilder.WalkableDictionary;
        var keys = walkable.Keys.ToList();
        foreach (var unit in units)
        {
            // Find an unoccupied walkable tile
            TileSD tile = null;
            for (int attempts = 0; attempts < 100; attempts++)
            {
                var candidate = walkable[keys[Random.Range(0, keys.Count)]];
                if (!candidate.Occupied)
                {
                    tile = candidate;
                    break;
                }
            }
            if (tile != null)
            {
                unit.Movement.SetStartPos(tile);
            }
        }
    }

    private void CheckPhaseEnded()
    {
        foreach (var unit in units)
        {
            /*// Skip dead units
            if (unit.Damageable.CurrentHealth <= 0)
                continue;*/
            if (!unit.ActionHandler.TurnPlayed)
            {
                return;
            }
        }
        GameManager.Instance.TurnManager.SwitchPhase();
    }


    public void LockUnits()
    {
        foreach (var unit in units)
        {
            unit.Selector.Selectable = false;
        }
    }

    public void UnLockUnits()
    {
        foreach (var unit in units)
        {
            unit.Selector.Selectable = true;
        }
    }

    public void PlayerPhaseReset()
    {
        foreach (var item in units)
        {
            item.ActionHandler.BeginPhase();
            item.Movement.ResetSpeed();
        }
    }

}
