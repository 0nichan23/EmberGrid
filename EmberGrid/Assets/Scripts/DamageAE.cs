using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AE/Damage", fileName = "DamageEffect")]
public class DamageAE : ActionEffect
{
    [SerializeField] private DamageType type;
    [SerializeField] private int baseDamage;
    [SerializeField] private BaseStat[] scaleStats;
    public override void InvokeEffect(Unit target, Unit User)
    {
        DamageHandler dmg = new DamageHandler(type, baseDamage);
        foreach (BaseStat stat in scaleStats)
        {
            dmg.AddMod(User.Stats.GetStatMod(stat));
        }
        target.Damageable.TakeDamage(dmg, User);
    }

}
