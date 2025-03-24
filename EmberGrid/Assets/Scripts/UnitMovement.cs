using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class UnitMovement
{
    [SerializeField] private TileSD currentTile;
    [SerializeField] private int speed;//tiles per round
    public TileSD CurrentTile { get => currentTile; set => currentTile = value; }
    public int Speed { get => speed; set => speed = value; }

    public UnitMovement(int speed)
    {
        this.speed = speed;
    }

    public void DisplayReachableTiles()
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
                if (reach != null)
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
            reachable.BlackBlink();
        }
    }

}
