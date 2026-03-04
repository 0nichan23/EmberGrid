using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/Passives/StatBoost", fileName = "StatBoostPassive")]
public class StatBoostPassive : PassiveEffect
{
    [SerializeField] private BaseStat stat;
    [SerializeField] private int amount;

    public BaseStat Stat { get => stat; }
    public int Amount { get => amount; }

    public override void Apply(Unit target)
    {
        target.Stats.AddStatBonus(stat, amount);
    }

    public override void Remove(Unit target)
    {
        target.Stats.AddStatBonus(stat, -amount);
    }
}
