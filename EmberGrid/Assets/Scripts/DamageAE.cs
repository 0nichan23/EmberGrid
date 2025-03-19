using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AE/Damage", fileName = "DamageEffect")]
public class DamageAE : ActionEffect
{
    [SerializeField] private DamageType type;
    [SerializeField] private int baseDamage;

    public override void InvokeEffect(Unit target, Unit User)
    {
        target.Damageable.TakeDamage(new DamageHandler(type, baseDamage), User);
    }

}
