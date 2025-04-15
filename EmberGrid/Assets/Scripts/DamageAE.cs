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
            dmg.AddModFlat(User.Stats.GetStatFlatMod(stat));
        }
        dmg.AddModFlat(target.Stats.GetDamageSpecificResistance(type) * -1);
        dmg.AddModFlat(target.Stats.GetDamageReductionStat(type) * -1);

        target.Damageable.TakeDamage(dmg, User);
    }

    public override void InvokeDisplayEffect(Unit target, Unit User)
    {
        DamageHandler dmg = new DamageHandler(type, baseDamage);
        foreach (BaseStat stat in scaleStats)
        {
            dmg.AddModFlat(User.Stats.GetStatFlatMod(stat));
        }
        dmg.AddModFlat(target.Stats.GetDamageSpecificResistance(type) * -1);
        dmg.AddModFlat(target.Stats.GetDamageReductionStat(type) * -1);

        target.Damageable.TakeDamageDisplay(dmg, User);
    }


    public int GetDamageFromUnit(Unit User)
    {
        int total = baseDamage;
        foreach (BaseStat stat in scaleStats)
        {
            total += User.Stats.GetStatFlatMod(stat);
        }
        return total;
    }
}
