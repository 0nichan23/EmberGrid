using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStats
{
    [SerializeField] private StatData might; //damage with heavy melee weapons, 
    [SerializeField] private StatData finesse; //damage with light melee weapons and ranged weapons, critical hit rate
    [SerializeField] private StatData magic;//damage with all magical attacks
    [SerializeField] private StatData resilience;//damage reduction against all physical attacks
    [SerializeField] private StatData resistance;//damage reduction against all magical attacks
    [SerializeField] private StatData willpower;//more mana for casters, durabilty bonus for martials

    public int Might { get => might.value; set => might.value = value; }
    public int Finesse { get => finesse.value; set => finesse.value = value; }
    public int Magic { get => magic.value; set => magic.value = value; }
    public int Resilience { get => resilience.value; set => resilience.value = value; }
    public int Resistance { get => resistance.value; set => resistance.value = value; }
    public int Willpower { get => willpower.value; set => willpower.value = value; }


    private int physicalDefEx;
    private int magicalDefEx;
    private int criticalHitEx;
    private int damageBonusEx;

    //specific defences

    private Dictionary<DamageType, int> specificResistances = new Dictionary<DamageType, int>();

    public int PhysicalDefenceT => Resilience + physicalDefEx;
    public int MagicDefenceT => Resistance + magicalDefEx;
    public float CritHit => Mathf.Clamp((((float)Finesse / 2) + criticalHitEx), 0, 100);

    public int PhysicalDefEx { get => physicalDefEx; set => physicalDefEx = value; }
    public int MagicalDefEx { get => magicalDefEx; set => magicalDefEx = value; }
    public int CriticalHitEx { get => criticalHitEx; set => criticalHitEx = value; }
    public int DamageBonusEx { get => damageBonusEx; set => damageBonusEx = value; }
    public Dictionary<DamageType, int> SpecificResistances { get => specificResistances; }

    public void SetSpecificResistances(Dictionary<DamageType, int> resistances)
    {
        specificResistances = new Dictionary<DamageType, int>(resistances);
    }

    public UnitStats(BaseStats givenBaseStats)
    {
        this.might.value = givenBaseStats.Might;
        this.might.BaseStat = BaseStat.Might;

        this.finesse.value = givenBaseStats.Finesse;
        this.finesse.BaseStat = BaseStat.Finesse;

        this.magic.value = givenBaseStats.Magic;
        this.magic.BaseStat = BaseStat.Magic;

        this.resilience.value = givenBaseStats.Resilience;
        this.resilience.BaseStat = BaseStat.Resilience;

        this.resistance.value = givenBaseStats.Resistance;
        this.resistance.BaseStat = BaseStat.Resistance;

        this.willpower.value = givenBaseStats.Willpower;
        this.willpower.BaseStat = BaseStat.Willpower;

    }


    public void AddSpecificDamageResistance(DamageType type, int amount)
    {
        if (specificResistances.ContainsKey(type))
        {
            specificResistances[type] += amount;
        }
        else
        {
            specificResistances.Add(type, amount);
        }
    }

    public int GetDamageSpecificResistance(DamageType type)
    {
        if (specificResistances.ContainsKey(type))
        {
            return specificResistances[type];
        }
        return 0;
    }

    public int GetDamageReductionStat(DamageType type)
    {
        switch (type)
        {
            case DamageType.Piercing:
                return PhysicalDefenceT;

            case DamageType.Slashing:
                return PhysicalDefenceT;

            case DamageType.Bludgeoning:
                return PhysicalDefenceT;

            case DamageType.Fire:
                return MagicDefenceT;

            case DamageType.Cold:
                return MagicDefenceT;

            case DamageType.Electric:
                return MagicDefenceT;

            case DamageType.Acid:
                return MagicDefenceT;

            case DamageType.Poison:
                return MagicDefenceT;

            case DamageType.Necrotic:
                return MagicDefenceT;

            case DamageType.Radiant:
                return MagicDefenceT;
        }
        return 0;
    }

    

    public int GetStat(BaseStat stat)
    {
        switch (stat)
        {
            case BaseStat.Might:
                return Might;
            case BaseStat.Finesse:
                return Finesse;
            case BaseStat.Magic:
                return Magic;
            case BaseStat.Resilience:
                return Resilience;
            case BaseStat.Resistance:
                return Resistance;
            case BaseStat.Willpower:
                return Willpower;
        }
        return 0;
    }


    public float GetStatMod(BaseStat stat)
    {
        switch (stat)
        {
            case BaseStat.Might:
                return CalcStatMod(150, Might);
            case BaseStat.Finesse:
                return CalcStatMod(150, Finesse);
            case BaseStat.Magic:
                return CalcStatMod(150, Magic);
            case BaseStat.Resilience:
                return CalcStatDec(75, Resilience);
            case BaseStat.Resistance:
                return CalcStatDec(75, Resistance);
            case BaseStat.Willpower:
                return CalcStatMod(200, Willpower);
        }
        return 0;
    }



    public int GetStatFlatMod(BaseStat stat)
    {
        switch (stat)
        {
            case BaseStat.Might:
                return Might;
            case BaseStat.Finesse:
                return Finesse;
            case BaseStat.Magic:
                return Magic;
            case BaseStat.Resilience:
                return Resilience;
            case BaseStat.Resistance:
                return Resistance;
            case BaseStat.Willpower:
                return Willpower;
        }
        return 0;
    }


    private float CalcStatMod(float maximumBonus, int value)
    {
        return value * (maximumBonus / 100 / 20) + 1;
    }


    private float CalcStatDec(float maximumBonus, int value)
    {
        return 1 - (value * (maximumBonus / 100 / 20));
    }

}


[System.Serializable]
public struct StatData
{
    public BaseStat BaseStat;
    public int value;
}


public enum BaseStat
{
    Might,
    Finesse,
    Magic,
    Resilience,
    Resistance,
    Willpower
}

