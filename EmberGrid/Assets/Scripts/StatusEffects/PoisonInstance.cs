using UnityEngine;

public class PoisonInstance : StatusEffectInstance
{
    public PoisonInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
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

        float poisonPercent = Config.GetValue(StatusEffects.Poison);
        int damage = Mathf.RoundToInt(Owner.Damageable.MaxHealth * (poisonPercent / 100f) * Stacks);
        Owner.Damageable.TakeStatusDamage(damage, DamageType.Poison, Owner);
        Debug.Log($"{Owner.name} took {damage} poison damage ({Stacks} stacks)");

        TickAndRemoveIfDepleted();
    }
}
