using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSnapshot
{
    public Unit UnitRef;
    public Vector2Int TilePos;
    public int CurrentHealth;
    public int ActionPoints;
    public int SpeedLeft;
    public bool TurnPlayed;
    public ActiveMode Mode;
    public bool Selectable;

    public int PhysicalDefEx;
    public int MagicalDefEx;
    public int CriticalHitEx;
    public int DamageBonusEx;
    public Dictionary<DamageType, int> SpecificResistances;

    public static UnitSnapshot Capture(Unit unit)
    {
        return new UnitSnapshot
        {
            UnitRef = unit,
            TilePos = unit.Movement.CurrentTile.Pos,
            CurrentHealth = unit.Damageable.CurrentHealth,
            ActionPoints = unit.ActionHandler.ActionPoints,
            SpeedLeft = unit.Movement.SpeedLeft,
            TurnPlayed = unit.ActionHandler.TurnPlayed,
            Mode = unit.CurrentMode,
            Selectable = unit.Selector.Selectable,
            PhysicalDefEx = unit.Stats.PhysicalDefEx,
            MagicalDefEx = unit.Stats.MagicalDefEx,
            CriticalHitEx = unit.Stats.CriticalHitEx,
            DamageBonusEx = unit.Stats.DamageBonusEx,
            SpecificResistances = new Dictionary<DamageType, int>(unit.Stats.SpecificResistances)
        };
    }

    public void Restore()
    {
        Unit unit = UnitRef;

        unit.StopAllCoroutines();
        unit.Movement.CancelMovementMode();
        unit.WeaponHandler.CancelAttackMode();

        if (unit.Movement.CurrentTile != null)
        {
            unit.Movement.CurrentTile.UnSubUnit();
        }

        TileSD targetTile = GameManager.Instance.GridBuilder.GetTileFromPosition(
            TilePos, GameManager.Instance.GridBuilder.WalkableDictionary);

        if (targetTile == null)
        {
            targetTile = GameManager.Instance.GridBuilder.GetTileFromPosition(
                TilePos, GameManager.Instance.GridBuilder.TileDictionary);
        }

        if (targetTile != null)
        {
            unit.Movement.SetStartPos(targetTile);
        }

        unit.Damageable.SetHealth(CurrentHealth);
        unit.ActionHandler.ActionPoints = ActionPoints;
        unit.ActionHandler.TurnPlayed = TurnPlayed;
        unit.Movement.SpeedLeft = SpeedLeft;
        unit.CurrentMode = Mode;
        unit.Selector.Selectable = Selectable;

        unit.Stats.PhysicalDefEx = PhysicalDefEx;
        unit.Stats.MagicalDefEx = MagicalDefEx;
        unit.Stats.CriticalHitEx = CriticalHitEx;
        unit.Stats.DamageBonusEx = DamageBonusEx;
        unit.Stats.SetSpecificResistances(SpecificResistances);
    }
}
