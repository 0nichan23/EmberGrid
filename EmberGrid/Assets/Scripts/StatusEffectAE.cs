using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AE/Status", fileName = "StatusEffect")]
public class StatusEffectAE : ActionEffect
{
    public override void InvokeDisplayEffect(Unit target, Unit User)
    {
        Debug.LogWarning("StatusEffectAE not yet implemented");
    }

    public override void InvokeEffect(Unit target, Unit User)
    {
        Debug.LogWarning("StatusEffectAE not yet implemented");
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
    Stride
}
