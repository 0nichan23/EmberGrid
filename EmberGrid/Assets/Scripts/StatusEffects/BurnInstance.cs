using UnityEngine;

public class BurnInstance : StatusEffectInstance
{
    public BurnInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
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

        float burnFlat = Config.GetValue(StatusEffects.Burn);
        int damage = Mathf.RoundToInt(burnFlat * Stacks);
        Owner.Damageable.TakeStatusDamage(damage, DamageType.Fire, Owner);
        Debug.Log($"{Owner.name} took {damage} burn damage ({Stacks} stacks)");

        TickAndRemoveIfDepleted();
    }
}
