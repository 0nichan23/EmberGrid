using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class WeaponHandler
{
    [SerializeField] private Weapon weapon;
    private Unit owner;
    private TargetedActionData currentTargetData;
    public Weapon Weapon { get => weapon; }


    public WeaponHandler(Unit givenUnit, Weapon weapon)
    {
        this.weapon = weapon;
        owner = givenUnit;
        owner.OnDeselected.AddListener(CancelAttackMode);
    }

    public List<UnitAction> GetAvailableActions(int remainingAP)
    {
        var list = new List<UnitAction> { weapon.BasicAttack };
        if (weapon.Encounters != null && weapon.Encounters.Length > 0)
        {
            list.AddRange(weapon.Encounters);
        }

        if (weapon.Daily != null)
            list.Add(weapon.Daily);

        // filter by AP cost
        return list.Where(a => a.Cost <= remainingAP).ToList();
    }

    //for enemies.
    public void ExecuteActionAt(UnitAction action, Direction dir, TileSD sourceTile)
    {
        // 1) Calculate hitbox tiles
        var hitbox = GameManager.Instance.GridBuilder
                         .Targeter
                         .GetHitbox(action, dir, sourceTile.Pos);

        // 2) Apply the action to each tile
        GameManager.Instance.GridBuilder.HitTiles(owner, action, hitbox);

        // 3) Consume the action point
        owner.ActionHandler.ExpandAction();
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

    public void SetAttackMode(UnitAction action)
    {
        if (!owner.ActionHandler.CanTakeAction)
        {
            return;
        }

        owner.Movement.CancelMovementMode();

        TileSD[] reach = GameManager.Instance.GridBuilder.GetTilesInReach(owner.Movement.CurrentTile, action.Range);
        currentTargetData = new TargetedActionData(reach, action, owner, Direction.Right);



    }





    public void CancelAttackMode()
    {
        if (!ReferenceEquals(currentTargetData, null))
        {
            currentTargetData.Cancel();
            currentTargetData = null;
        }
    }

}

public class TargetedActionData
{
    private TileSD[] reach;
    private TileSD[] hitbox;
    private TileSD currentSource;
    private Direction dir;
    private UnitAction refAction;
    private Unit owner;


    public TargetedActionData(TileSD[] reach, UnitAction action, Unit owner, Direction dir)
    {
        this.reach = reach;

        this.refAction = action;
        this.owner = owner;
        this.dir = dir;

        GameManager.Instance.InputManager.RotLeft.AddListener(RotHitboxLeft);
        GameManager.Instance.InputManager.RotRight.AddListener(RotHitboxRight);

        foreach (var item in reach)
        {
            item.AttackOverlay();
            item.RefTile.OnTileClicked.AddListener(AttackTargetedTile);
            item.RefTile.OnTileHoveredRef.AddListener(SetCurrentSource);
            item.RefTile.OnTileUnHoveredRef.AddListener(ReleaseSource);
        }

        this.dir = dir;
    }

    private void SetHitbox()
    {
        ResetHitboxHighlight();
        if (!ReferenceEquals(currentSource, null))
        {
            hitbox = GameManager.Instance.GridBuilder.Targeter.GetHitbox(refAction, dir, currentSource.Pos);
            foreach (var item in hitbox)
            {
                item.TargetOverlay(owner, refAction);
            }
        }
    }

    private void RotHitboxLeft()
    {
        dir = Utilities.GetRotLeftDir(dir);
        SetHitbox();
    }
    private void RotHitboxRight()
    {
        dir = Utilities.GetRotRightDir(dir);
        SetHitbox();
    }

    private void SetCurrentSource(TileSD source)
    {
        currentSource = source;
        SetHitbox();
    }

    private void ReleaseSource(TileSD source)
    {
        ResetHitboxHighlight();
        currentSource = null;
    }

    private void ResetHitboxHighlight()
    {
        List<TileSD> reachlist = new List<TileSD>(reach);
        if (!ReferenceEquals(hitbox, null))
        {
            foreach (var item in hitbox)
            {
                item.ResetOverlay();
                if (reachlist.Contains(item))
                {
                    item.AttackOverlay();
                }
            }
        }
    }

    private void AttackTargetedTile()
    {
        GameManager.Instance.GridBuilder.HitTiles(owner, refAction, hitbox);
        owner.ActionHandler.ExpandAction();
        Cancel();
    }

    public void Cancel()
    {
        GameManager.Instance.InputManager.RotLeft.RemoveListener(RotHitboxLeft);
        GameManager.Instance.InputManager.RotRight.RemoveListener(RotHitboxRight);

        foreach (var item in reach)
        {
            item.RefTile.OnTileClicked.RemoveListener(AttackTargetedTile);
            item.RefTile.OnTileHoveredRef.RemoveListener(SetCurrentSource);
            item.RefTile.OnTileUnHoveredRef.RemoveListener(ReleaseSource);

            item.ResetOverlay();
        }
    }

}

