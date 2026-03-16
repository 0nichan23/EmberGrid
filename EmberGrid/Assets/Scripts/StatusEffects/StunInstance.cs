using UnityEngine;

public class StunInstance : StatusEffectInstance
{
    public StunInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
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

        // Stun blocking is handled in UnitActionHandler.BeginPhase()
        Debug.Log($"{Owner.name} stun ticked down");
        TickAndRemoveIfDepleted();
    }
}
