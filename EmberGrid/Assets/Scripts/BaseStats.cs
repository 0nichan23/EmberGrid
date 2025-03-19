using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Stats", menuName = "Base Stats")]
public class BaseStats : ScriptableObject 
{
    [SerializeField] private int maxHealth;
    [Range(1, 20)][SerializeField] private int might;
    [Range(1, 20)][SerializeField] private int finesse;
    [Range(1, 20)][SerializeField] private int magic;
    [Range(1, 20)][SerializeField] private int resilience;
    [Range(1, 20)][SerializeField] private int resistance;
    [Range(1, 20)][SerializeField] private int willpower;

    public int Might { get => might;}
    public int Finesse { get => finesse;}
    public int Magic { get => magic;}
    public int Resilience { get => resilience;}
    public int Resistance { get => resistance;}
    public int Willpower { get => willpower;}
    public int MaxHealth { get => maxHealth;}
}
