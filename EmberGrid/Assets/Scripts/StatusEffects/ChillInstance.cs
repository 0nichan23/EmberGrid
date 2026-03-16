using UnityEngine;

public class ChillInstance : StatusEffectInstance
{
    public ChillInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
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

        // Speed reduction is passive via UnitMovement.GetEffectiveSpeed()
        Debug.Log($"{Owner.name} chill ticked down");
        TickAndRemoveIfDepleted();
    }
}
