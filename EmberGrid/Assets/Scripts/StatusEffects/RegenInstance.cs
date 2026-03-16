using UnityEngine;

public class RegenInstance : StatusEffectInstance
{
    public RegenInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
        : base(owner, type, stacks, config) { }

    public override void OnApplied()
    {
        if (IsSubscribed) return;
        StatusEffectManager.OnUnitPhaseStart += HandlePhaseStart;
        IsSubscribed = true;
    }

    public override void OnRemoved()
    {
        if (!IsSubscribed) return;
        StatusEffectManager.OnUnitPhaseStart -= HandlePhaseStart;
        IsSubscribed = false;
    }

    private void HandlePhaseStart(Unit unit)
    {
        if (unit != Owner) return;

        float regenPercent = Config.GetValue(StatusEffects.Regen);
        int healAmount = Mathf.RoundToInt(Owner.Damageable.MaxHealth * (regenPercent / 100f) * Stacks);
        Owner.Damageable.RestoreHealth(healAmount);
        Debug.Log($"{Owner.name} regenerated {healAmount} HP ({Stacks} stacks)");

        TickAndRemoveIfDepleted();
    }
}
