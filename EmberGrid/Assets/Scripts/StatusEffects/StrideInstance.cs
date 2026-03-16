using UnityEngine;

public class StrideInstance : StatusEffectInstance
{
    public StrideInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
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

        // Stride is removed entirely at phase end
        // Speed doubling is passive via UnitMovement.GetEffectiveSpeed()
        Debug.Log($"{Owner.name} stride expired");
        Owner.RemoveStatusEffect(StatusEffects.Stride);
    }
}
