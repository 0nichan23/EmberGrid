using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
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

    public void Attack(UnitAction action)
    {
        if (!owner.ActionHandler.CanTakeAction)
        {
            return;
        }


        if (!action.Targeted)
        {
            GameManager.Instance.GridBuilder.HitTiles(owner, action);
        }
        else
        {
            TileSD[] reach = GameManager.Instance.GridBuilder.GetTilesInReach(owner.Movement.CurrentTile, action.Range);
            TargetedActionData targetData = new TargetedActionData(reach, action, owner);
            //open some sort of selection panel.
            //can attack any tile as long as its in range. 
        }

        owner.ActionHandler.ExpandAction();

    }

}

public class TargetedActionData
{
    private TileSD[] reach;
    private UnitAction refAction;
    private Unit owner;

    public TargetedActionData(TileSD[] reach, UnitAction action, Unit owner)
    {
        this.reach = reach;
        this.refAction = action;
        this.owner = owner;
        foreach (var item in reach)
        {
            item.AttackOverlay();
            item.RefTile.OnTileClicked.AddListener(AttackTargetedTile);
        }
    }

    private void AttackTargetedTile(TileSD sd)
    {
        GameManager.Instance.GridBuilder.HitTiles(owner, refAction, sd);

        foreach (var item in reach)
        {
            item.RefTile.OnTileClicked.RemoveListener(AttackTargetedTile);
            item.ResetOverlay();
        }
    }

}

