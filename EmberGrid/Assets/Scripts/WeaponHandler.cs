using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class WeaponHandler
{
    [SerializeField] private Weapon weapon;
    private Unit owner;
    public Weapon Weapon { get => weapon; }


    public WeaponHandler(Unit givenUnit, Weapon weapon)
    {
        this.weapon = weapon;
        owner = givenUnit;
    }

    public int GetBasicAttackDamage()
    {
        int total = 0;

        foreach (var item in weapon.BasicAttack.Effects)
        {
            if (item is not DamageAE)
            {
                continue;
            }
            else
            {
                total += (item as DamageAE).GetDamageFromUnit(owner);
            }
        }
        return total;
    }


}
