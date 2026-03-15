using UnityEngine;

[CreateAssetMenu(menuName = "AE/Status", fileName = "StatusEffect")]
public class StatusEffectAE : ActionEffect
{
    [SerializeField] private StatusEffects effectType;
    [SerializeField] private int stacksToApply = 1;

    public override void InvokeEffect(Unit target, Unit User)
    {
        target.AddStatusEffect(effectType, stacksToApply);
        Debug.Log($"{User.name} applied {stacksToApply} stack(s) of {effectType} to {target.name}");
    }

    public override void InvokeDisplayEffect(Unit target, Unit User)
    {
        Debug.Log($"{User.name} would apply {stacksToApply} stack(s) of {effectType} to {target.name}");
    }
}

public enum StatusEffects
{
    Poison,
    Burn,
    Chill,
    Shock,
    Bleed,
    Stun,
    Regen,
    Stride,
    Haste
}
