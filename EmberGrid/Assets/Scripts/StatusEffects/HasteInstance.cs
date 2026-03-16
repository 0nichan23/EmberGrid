using UnityEngine;

public class HasteInstance : StatusEffectInstance
{
    public HasteInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
        : base(owner, type, stacks, config) { }

    public override void OnApplied()
    {
        if (IsSubscribed) return;
        StatusEffectManager.OnUnitPhaseStart += HandlePhaseStart;
        StatusEffectManager.OnUnitPhaseEnd += HandlePhaseEnd;
        IsSubscribed = true;
    }

    public override void OnRemoved()
    {
        if (!IsSubscribed) return;
        StatusEffectManager.OnUnitPhaseStart -= HandlePhaseStart;
        StatusEffectManager.OnUnitPhaseEnd -= HandlePhaseEnd;
        IsSubscribed = false;
    }

    private void HandlePhaseStart(Unit unit)
    {
        if (unit != Owner) return;

        Owner.ActionHandler.ActionPoints += 1;
        Debug.Log($"{Owner.name} is hasted, gained +1 AP");
    }

    private void HandlePhaseEnd(Unit unit)
    {
        if (unit != Owner) return;

        // Haste is removed entirely at phase end
        Debug.Log($"{Owner.name} haste expired");
        Owner.RemoveStatusEffect(StatusEffects.Haste);
    }
}
