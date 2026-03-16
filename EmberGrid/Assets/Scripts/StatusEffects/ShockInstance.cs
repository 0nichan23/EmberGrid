using UnityEngine;

public class ShockInstance : StatusEffectInstance
{
    public ShockInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
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

        // Damage amplification is passive via Damageable.TakeDamage()
        Debug.Log($"{Owner.name} shock ticked down");
        TickAndRemoveIfDepleted();
    }
}
