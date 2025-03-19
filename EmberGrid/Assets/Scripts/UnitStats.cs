using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStats
{
    [SerializeField] private int might;
    [SerializeField] private int finesse;
    [SerializeField] private int magic;
    [SerializeField] private int resilience;
    [SerializeField] private int resistance;
    [SerializeField] private int willpower;
 
    public int Might { get => Mathf.Clamp(might, 1, 20); set => might = value; }
    public int Finesse { get => Mathf.Clamp(finesse, 1, 20); set => finesse = value; }
    public int Magic { get => Mathf.Clamp(magic, 1, 20); set => magic = value; }
    public int Resilience { get => Mathf.Clamp(resilience, 1, 20); set => resilience = value; }
    public int Resistance { get => Mathf.Clamp(resistance, 1, 20); set => resistance = value; }
    public int Willpower { get => Mathf.Clamp(willpower, 1, 20); set => willpower = value; }

    public UnitStats(BaseStats givenBaseStats)
    {
        this.might = givenBaseStats.Might;
        this.finesse = givenBaseStats.Finesse;
        this.magic = givenBaseStats.Magic;
        this.resilience = givenBaseStats.Resilience;
        this.resistance = givenBaseStats.Resistance;
        this.willpower = givenBaseStats.Willpower;
    }

}
