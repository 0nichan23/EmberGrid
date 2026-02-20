using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AE/Status", fileName = "StatusEffect")]
public class StatusEffectAE : ActionEffect
{
    public override void InvokeDisplayEffect(Unit target, Unit User)
    {
        throw new System.NotImplementedException();
    }

    public override void InvokeEffect(Unit target, Unit User)
    {
        throw new System.NotImplementedException();
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
