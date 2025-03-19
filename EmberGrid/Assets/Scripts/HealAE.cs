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
}
