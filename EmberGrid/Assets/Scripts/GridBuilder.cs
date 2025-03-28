using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private CustomTile[] tiles;

    [SerializeField] private Unit tempStartUnit;
    [SerializeField] private Unit tempStartUnit2;

    private Pathfinder pathfinder;



    private Dictionary<Vector2Int, TileSD> tileDictionary = new Dictionary<Vector2Int, TileSD>();
    private Dictionary<Vector2Int, TileSD> walkableDictionary = new Dictionary<Vector2Int, TileSD>();

    public Dictionary<Vector2Int, TileSD> TileDictionary { get => tileDictionary; }
    public Dictionary<Vector2Int, TileSD> WalkableDictionary { get => walkableDictionary;}
    public Pathfinder Pathfinder { get => pathfinder; }

    private void Start()
    {
        Application.targetFrameRate = 120;
        GenerateGrid();
        PlaceTempUnit();
        pathfinder = new Pathfinder();
    }
    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                CustomTile tile = tiles[Random.Range(0, tiles.Length)];
                tilemap.SetTile(tilePosition, tile);
                if (tile.Prefab)
                {
                    TilePrefab tilePrefab = Instantiate(tile.Prefab, tilemap.CellToWorld(tilePosition), Quaternion.identity);
                    tilePrefab.transform.SetParent(tilemap.transform);
                    TileSD tilesd = new TileSD(tilePrefab, (Vector2Int)tilePosition);
                    tileDictionary.Add(new Vector2Int(x, y), tilesd);
                    if (tilePrefab.gameObject.CompareTag("Ground"))
                    {
                        walkableDictionary.Add(new Vector2Int(x, y), tilesd);
                    }
                }
            }
        }
    }


    public TileSD GetTileFromPosition(Vector2Int pos, Dictionary<Vector2Int, TileSD> map = null)
    {
        if (map.ContainsKey(pos))
        {
            return map[pos];
        }
        else
        {
            return null;
        }
    }


    public void HitTiles(Unit user, UnitAction action)
    {
        Vector2Int userTilePos = user.Movement.CurrentTile.Pos;

        foreach (var target in action.Targets)
        {
            TileSD currentTile = GetTileFromPosition(userTilePos + target, tileDictionary);
            if (!ReferenceEquals(currentTile, null))
            {
                tilemap.SetColor((Vector3Int)userTilePos, Color.red);
                currentTile.HitTile(user, action);
            }
        }
    }

    public List<TileSD> GetNeighbours(TileSD tile, Dictionary<Vector2Int, TileSD> map)
    {
        List<TileSD> neighbours = new List<TileSD>();

        TileSD tileN = GetTileFromPosition(tile.Pos + new Vector2Int(1, 0), map);
        if (!ReferenceEquals(tileN, null))
        {
            neighbours.Add(tileN);
        }
        tileN = GetTileFromPosition(tile.Pos + new Vector2Int(-1, 0), map);
        if (!ReferenceEquals(tileN, null))
        {
            neighbours.Add(tileN);
        }
        tileN = GetTileFromPosition(tile.Pos + new Vector2Int(0, 1), map);
        if (!ReferenceEquals(tileN, null))
        {
            neighbours.Add(tileN);
        }
        tileN = GetTileFromPosition(tile.Pos + new Vector2Int(0, -1), map);
        if (!ReferenceEquals(tileN, null))
        {
            neighbours.Add(tileN);
        }
        return neighbours;
    }

    private void PlaceTempUnit()
    {
        TileSD tile = GetTileFromPosition(new Vector2Int(width / 2, height / 2), walkableDictionary);
        if (tile != null)
        {
            tile.SubUnit(tempStartUnit);
            tempStartUnit.transform.position = new Vector3Int(tile.Pos.x, tile.Pos.y, 0);
        }
        TileSD tile2 = GetTileFromPosition(tempStartUnit.Movement.CurrentTile.Pos + new Vector2Int(1, 0), tileDictionary);
        if (tile2 != null)
        {
            tile2.SubUnit(tempStartUnit2);
            tempStartUnit2.transform.position = new Vector3Int(tile2.Pos.x, tile2.Pos.y, 0);
        }
    }


   /* [ContextMenu("Test path")]
    public void TestPath()
    {
       List<TileSD> foundPath =  pathfinder.FindPathToDest(tempStartUnit.Movement.CurrentTile, GetTileFromPosition(new Vector2Int(0, 0), tileDictionary), walkableDictionary);

        if (ReferenceEquals(foundPath, null))
        {
            Debug.Log("Path unreachable");
            return;
        }
        foreach (TileSD tile in foundPath)
        {
            tile.MoveOverlay();
        }
    }*/
}

[System.Serializable]
public class TileSD : IHeapItem<TileSD>
{
    private TilePrefab refTile;
    private Unit subscribedUnit;
    [SerializeField] private Vector2Int pos;
    //to physically position a unit on a tile simply use the position as a v3int 
    public TileSD PathParent;
    private int heapIndex;
    public int costToEnd;
    public int costToStart;
    public int totalCost => costToEnd + costToStart;

    public bool Occupied => subscribedUnit != null;

    public TilePrefab RefTile { get => refTile; }
    public Unit SubscribedUnit { get => subscribedUnit; }
    public Vector2Int Pos { get => pos; }
    public int HeapIndex { get => heapIndex; set => heapIndex = value; }

    public TileSD(TilePrefab tile, Vector2Int pos)
    {
        refTile = tile;
        this.pos = pos;
        tile.CacheSD(this);
    }

    public void SubUnit(Unit subscribedUnit)
    {
        this.subscribedUnit = subscribedUnit;
        subscribedUnit.Movement.CurrentTile = this;
    }

    public void UnSubUnit()
    {
        this.subscribedUnit = null;
    }

    public void HitTile(Unit user, UnitAction action)
    {
        Debug.Log($"{pos} was hit by {user}");
        refTile.RedBlink();
        if (!ReferenceEquals(subscribedUnit, null))
        {
            subscribedUnit.Damageable.GetHit(action, user);
        }
    }

    public void MoveOverlay()
    {
        refTile.SetMoveOverlay();
    }

    public void ResetOverlay()
    {
        RefTile.ResetOverlay();
    }

    public int CompareTo(TileSD other)
    {
        int compare = totalCost.CompareTo(other.totalCost);
        if (compare == 0)
        {
            compare = costToEnd.CompareTo(other.costToEnd);
        }
        return -compare;
    }
}
