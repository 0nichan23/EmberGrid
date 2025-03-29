using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitMovement
{
    [SerializeField] private TileSD currentTile;
    [SerializeField] private int speed;//tiles per round
    private TileSD[] currentReach;
    private Unit owner;
    public TileSD CurrentTile { get => currentTile; set => currentTile = value; }
    public int Speed { get => speed; set => speed = value; }
    public Unit Owner { get => owner; }

    public UnitMovement(Unit givenOwner, int speed)
    {
        owner = givenOwner;
        this.speed = speed;
    }

    public void SetReachableTiles()
    {
        List<TileSD> reachables = new List<TileSD>();

        for (int x = -speed; x <= speed; x++)
        {
            for (int y = -speed; y <= speed; y++)
            {
                Vector2Int pos = new Vector2Int(currentTile.Pos.x + x, currentTile.Pos.y + y);

                if (Pathfinder.GetDistanceOfTiles(currentTile.Pos, pos) > speed)
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

            if (path == null || path.Count > speed)
            {
                reachables.RemoveAt(i);
            }
        }

        foreach (var reachable in reachables)
        {
            reachable.MoveOverlay();
            reachable.RefTile.OnTileClicked.AddListener(MoveUnitToTile);
        }

        currentReach = reachables.ToArray();
    }




    private void MoveUnitToTile(TileSD tile)
    {
        if (!ReferenceEquals(CurrentTile, null))
        {
            currentTile.UnSubUnit();
        }
        owner.transform.position = new Vector3Int(tile.Pos.x, tile.Pos.y, 0);
        tile.SubUnit(owner);
        foreach (var item in currentReach)
        {
            item.RefTile.OnTileClicked.RemoveListener(MoveUnitToTile);
            item.ResetOverlay();
        }
    }
}
