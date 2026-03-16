using System;
using UnityEngine;

public class ShockInstance : StatusEffectInstance
{
    public ShockInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
        : base(owner, type, stacks, config) { }

    public override void OnApplied()
    {
        if (IsSubscribed) return;
        StatusEffectManager.OnUnitPhaseEnd += HandlePhaseEnd;
        Owner.Damageable.OnTakeDamage.AddListener(ApplyDamageIncreaseOnGetHit);
        IsSubscribed = true;
    }

    private void ApplyDamageIncreaseOnGetHit(DamageHandler dmg, DamageDealer dealer, Damageable target)
    {
        float mod = (GameManager.Instance.StatusEffectManager.Config.GetValue(StatusEffects.Shock) / 100) + 1;
        dmg.AddMod(mod);
    }

    public override void OnRemoved()
    {
        if (!IsSubscribed) return;
        StatusEffectManager.OnUnitPhaseEnd -= HandlePhaseEnd;
        Owner.Damageable.OnTakeDamage.RemoveListener(ApplyDamageIncreaseOnGetHit);
        IsSubscribed = false;
    }

    private void HandlePhaseEnd(Unit unit)
    {
        if (unit != Owner) return;
        
        Debug.Log($"{Owner.name} shock ticked down");
        TickAndRemoveIfDepleted();
    }
}
