using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private WeaponType type;
    [SerializeField] private UnitAction basicAttack;

    public UnitAction BasicAttack { get => basicAttack;}
}

public enum WeaponType
{
    HeavyMelee,
    LightMelee,
    Ranged, 
    Implement
}