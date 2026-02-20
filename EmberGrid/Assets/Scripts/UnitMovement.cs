using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitMovement
{
    [SerializeField] private TileSD currentTile;
    [SerializeField] private int speed;//tiles per round
    private TileSD[] currentReach;
    private Unit owner;
    private int speedLeft;
    public TileSD CurrentTile { get => currentTile; set => currentTile = value; }
    public int Speed { get => speed; set => speed = value; }
    public Unit Owner { get => owner; }
    public bool IsMoving { get; private set; }
    public TileSD[] CurrentReach { get => currentReach; }

    public UnitMovement(Unit givenOwner, int speed)
    {
        owner = givenOwner;
        this.speed = speed;
        speedLeft = speed;
        givenOwner.OnDeselected.AddListener(CancelMovementMode);
    }

    public void ResetSpeed()
    {
        speedLeft = speed;
    }


    public void CancelMovementMode()
    {
        if (!ReferenceEquals(currentReach, null) && currentReach.Length > 0)
        {
            foreach (var kvp in currentReach)
            {
                kvp.ResetOverlay();
                kvp.RefTile.OnTileClickedRef.RemoveListener(MoveUnitToTile);
            }
            currentReach = null;
        }
    }

    public void SetMovementMode()
    {
        if (owner.CurrentMode == ActiveMode.MovementMode)
        {
            //cant repeat mode subsciriptions twice
            return;
        }
        owner.CurrentMode = ActiveMode.MovementMode;

        owner.WeaponHandler.CancelAttackMode();
        List<TileSD> reachables = new List<TileSD>();

        for (int x = -speedLeft; x <= speedLeft; x++)
        {
            for (int y = -speedLeft; y <= speedLeft; y++)
            {
                Vector2Int pos = new Vector2Int(currentTile.Pos.x + x, currentTile.Pos.y + y);

                if (Pathfinder.GetDistanceOfTiles(currentTile.Pos, pos) > speedLeft)
                    continue;

                TileSD reach = GameManager.Instance.GridBuilder.GetTileFromPosition(pos, GameManager.Instance.GridBuilder.WalkableDictionary);
                if (reach != null && !reach.Occupied)
                {
                    reachables.Add(reach);
                }
            }
        }

        for (int i = reachables.Count - 1; i >= 0; i--)
        {
            var tile = reachables[i];
            var path = GameManager.Instance.GridBuilder.Pathfinder.FindPathToDest(
                currentTile, tile, GameManager.Instance.GridBuilder.WalkableDictionary);

            if (path == null || path.Count > speedLeft)
            {
                reachables.RemoveAt(i);
            }
        }

        if (owner is Hero)
        {
            foreach (var reachable in reachables)
            {
                reachable.MoveOverlay();
                reachable.RefTile.OnTileClickedRef.AddListener(MoveUnitToTile);
            }
        }

        currentReach = reachables.ToArray();
    }




    private void MoveUnitToTile(TileSD tile)
    {
        var path = GameManager.Instance.GridBuilder.Pathfinder.FindPathToDest(
                currentTile, tile, GameManager.Instance.GridBuilder.WalkableDictionary);

        speedLeft -= Pathfinder.GetDistanceOfTiles(currentTile.Pos, tile.Pos);

        if (!ReferenceEquals(CurrentTile, null))
        {
            currentTile.UnSubUnit();
        }
        //owner.transform.position = new Vector3Int(tile.Pos.x, tile.Pos.y, 0);
        owner.StartCoroutine(WalkAlongPath(path));
        tile.SubUnit(owner);
        foreach (var item in currentReach)
        {
            item.RefTile.OnTileClickedRef.RemoveListener(MoveUnitToTile);
            item.ResetOverlay();
        }
    }

    public void SetStartPos(TileSD tile)
    {
        owner.transform.position = new Vector3Int(tile.Pos.x, tile.Pos.y, 0);
        tile.SubUnit(owner);
    }


    private IEnumerator WalkAlongPath(List<TileSD> path)
    {
        foreach (var target in path)
        {
            yield return owner.StartCoroutine(MoveToPosition(target.RefTile.transform.position));
        }
    }

    private IEnumerator MoveToPosition(Vector3 pos)
    {
        while (Vector3.Distance(owner.transform.position, pos) > 0.05f)
        {
            owner.transform.position = Vector3.MoveTowards(owner.transform.position, pos, 2 * Time.deltaTime);
            yield return null;
        }
        owner.transform.position = pos; // Snap to final position
    }

    //for enemies. 
    public IEnumerator MoveAlongPath(List<TileSD> path)
    {
        IsMoving = true;
        foreach (var step in path)
        {
            yield return owner.StartCoroutine(WalkAlongPath(new List<TileSD> { step }));
        }
        IsMoving = false;
    }

}
