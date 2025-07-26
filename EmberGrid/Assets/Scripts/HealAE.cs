using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AE/Heal", fileName = "HealEffect")]
public class HealAE : ActionEffect
{
    [SerializeField] private int baseDamage;

    public override void InvokeEffect(Unit target, Unit User)
    {
        target.Damageable.RestoreDamage(baseDamage, User);
    }

    public int GetHealFromUnit(Unit user, Unit target)
    {
        return Mathf.Clamp(target.Damageable.MaxHealth - target.Damageable.CurrentHealth, 0, baseDamage);
       
    }

}
