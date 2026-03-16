using UnityEngine;

public class BleedInstance : StatusEffectInstance
{
    public BleedInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
        : base(owner, type, stacks, config) { }

    public override void OnApplied()
    {
        if (IsSubscribed) return;
        StatusEffectManager.OnUnitPhaseEnd += HandlePhaseEnd;
        IsSubscribed = true;
    }

    public override void OnRemoved()
    {
        if (!IsSubscribed) return;
        StatusEffectManager.OnUnitPhaseEnd -= HandlePhaseEnd;
        IsSubscribed = false;
    }

    private void HandlePhaseEnd(Unit unit)
    {
        if (unit != Owner) return;

        float bleedPercent = Config.GetValue(StatusEffects.Bleed);
        int damage = Mathf.RoundToInt(Owner.Damageable.MaxHealth * (bleedPercent / 100f) * Stacks);
        Owner.Damageable.TakeStatusDamage(damage, DamageType.Slashing, Owner);
        Debug.Log($"{Owner.name} took {damage} bleed damage ({Stacks} stacks)");

        TickAndRemoveIfDepleted();
    }
}
