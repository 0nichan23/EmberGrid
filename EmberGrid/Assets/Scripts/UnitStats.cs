using System;
using UnityEngine;

[System.Serializable]
public class UnitStats
{
    [SerializeField] private StatData might; //damage with heavy melee weapons, access to more powerful melee weapons
    [SerializeField] private StatData finesse; //damage with light melee weapons and ranged weapons, critical hit rate
    [SerializeField] private StatData magic;//damage with all magical attacks
    [SerializeField] private StatData resilience;//damage reduction against all physical attacks
    [SerializeField] private StatData resistance;//damage reduction against all magical attacks
    [SerializeField] private StatData willpower;//more mana for casters, durabilty bonus for martials
                                                                                            
    public int Might { get => Mathf.Clamp(might.value, 1, 20); set => might.value = value; }
    public int Finesse { get => Mathf.Clamp(finesse.value, 1, 20); set => finesse.value = value; }
    public int Magic { get => Mathf.Clamp(magic.value, 1, 20); set => magic.value = value; }
    public int Resilience { get => Mathf.Clamp(resilience.value, 1, 20); set => resilience.value = value; }
    public int Resistance { get => Mathf.Clamp(resistance.value, 1, 20); set => resistance.value = value; }
    public int Willpower { get => Mathf.Clamp(willpower.value, 1, 20); set => willpower.value = value; }


    public float PhysicalDefence => 1 - GetStatMod(BaseStat.Resilience);
    public float MagicDefence => 1 - GetStatMod(BaseStat.Resistance);
    public float CritHit => 60 * ((float)Finesse / 20);

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
                return CalcStatDec(70, Resilience);
            case BaseStat.Resistance:
                return CalcStatDec(70, Resistance);
            case BaseStat.Willpower:
                return CalcStatMod(200, Willpower);
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