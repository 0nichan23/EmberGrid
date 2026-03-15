using UnityEngine;

public static class StatusEffectProcessor
{
    /// <summary>
    /// Called at the START of a unit's phase.
    /// Handles: Regen (heal), Haste (grant AP), Stride (boost speed).
    /// </summary>
    public static void ProcessPhaseStart(Unit unit, StatusEffectConfig config)
    {
        // Regen: heal %maxHP * stacks, then lose 1 stack
        int regenStacks = unit.GetStatusStacks(StatusEffects.Regen);
        if (regenStacks > 0)
        {
            float regenPercent = config.GetValue(StatusEffects.Regen);
            int healAmount = Mathf.RoundToInt(unit.Damageable.MaxHealth * (regenPercent / 100f) * regenStacks);
            unit.Damageable.RestoreHealth(healAmount);
            unit.TickStatusEffect(StatusEffects.Regen);
            Debug.Log($"{unit.name} regenerated {healAmount} HP ({regenStacks} stacks)");
        }

        // Haste: grant +1 AP (applied at phase start, removed at phase end)
        if (unit.HasStatus(StatusEffects.Haste))
        {
            unit.ActionHandler.ActionPoints += 1;
            Debug.Log($"{unit.name} is hasted, gained +1 AP");
        }

        // Stride: double speed (applied at phase start, removed at phase end)
        // Speed modifier is handled passively in UnitMovement.GetEffectiveSpeed()
        if (unit.HasStatus(StatusEffects.Stride))
        {
            Debug.Log($"{unit.name} has stride, movement doubled");
        }
    }

    /// <summary>
    /// Called at the END of a unit's phase.
    /// Handles: Poison/Burn/Bleed (DOT damage), Chill/Shock (lose stack), Stun (lose stack), Haste/Stride (remove).
    /// </summary>
    public static void ProcessPhaseEnd(Unit unit, StatusEffectConfig config)
    {
        // Poison: deal %maxHP * stacks as Poison damage, lose 1 stack
        int poisonStacks = unit.GetStatusStacks(StatusEffects.Poison);
        if (poisonStacks > 0)
        {
            float poisonPercent = config.GetValue(StatusEffects.Poison);
            int damage = Mathf.RoundToInt(unit.Damageable.MaxHealth * (poisonPercent / 100f) * poisonStacks);
            unit.Damageable.TakeStatusDamage(damage, DamageType.Poison, unit);
            unit.TickStatusEffect(StatusEffects.Poison);
            Debug.Log($"{unit.name} took {damage} poison damage ({poisonStacks} stacks)");
        }

        // Burn: deal flat fire damage * stacks, lose 1 stack
        int burnStacks = unit.GetStatusStacks(StatusEffects.Burn);
        if (burnStacks > 0)
        {
            float burnFlat = config.GetValue(StatusEffects.Burn);
            int damage = Mathf.RoundToInt(burnFlat * burnStacks);
            unit.Damageable.TakeStatusDamage(damage, DamageType.Fire, unit);
            unit.TickStatusEffect(StatusEffects.Burn);
            Debug.Log($"{unit.name} took {damage} burn damage ({burnStacks} stacks)");
        }

        // Bleed: deal %maxHP * stacks as Slashing damage, lose 1 stack
        int bleedStacks = unit.GetStatusStacks(StatusEffects.Bleed);
        if (bleedStacks > 0)
        {
            float bleedPercent = config.GetValue(StatusEffects.Bleed);
            int damage = Mathf.RoundToInt(unit.Damageable.MaxHealth * (bleedPercent / 100f) * bleedStacks);
            unit.Damageable.TakeStatusDamage(damage, DamageType.Slashing, unit);
            unit.TickStatusEffect(StatusEffects.Bleed);
            Debug.Log($"{unit.name} took {damage} bleed damage ({bleedStacks} stacks)");
        }

        // Chill: lose 1 stack (speed reduction is passive)
        if (unit.HasStatus(StatusEffects.Chill))
        {
            unit.TickStatusEffect(StatusEffects.Chill);
            Debug.Log($"{unit.name} chill ticked down");
        }

        // Shock: lose 1 stack (damage amplification is passive)
        if (unit.HasStatus(StatusEffects.Shock))
        {
            unit.TickStatusEffect(StatusEffects.Shock);
            Debug.Log($"{unit.name} shock ticked down");
        }

        // Stun: lose 1 stack
        if (unit.HasStatus(StatusEffects.Stun))
        {
            unit.TickStatusEffect(StatusEffects.Stun);
            Debug.Log($"{unit.name} stun ticked down");
        }

        // Haste: remove entirely at end of phase
        if (unit.HasStatus(StatusEffects.Haste))
        {
            unit.RemoveStatusEffect(StatusEffects.Haste);
            Debug.Log($"{unit.name} haste expired");
        }

        // Stride: remove entirely at end of phase
        if (unit.HasStatus(StatusEffects.Stride))
        {
            unit.RemoveStatusEffect(StatusEffects.Stride);
            Debug.Log($"{unit.name} stride expired");
        }
    }
}
