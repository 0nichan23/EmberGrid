using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    public List<TileSD> FindPathToDest(TileSD startingPoint, TileSD destenation, Dictionary<Vector2Int, TileSD> map)
    {
        // Reset stale pathfinding data from previous calls
        foreach (var tile in map.Values)
        {
            tile.costToStart = 0;
            tile.costToEnd = 0;
            tile.PathParent = null;
            tile.HeapIndex = 0;
        }

        var openList = new Heap<TileSD>(map.Count);
        var closedList = new List<TileSD>();
        TileSD currentTile;
        openList.Add(startingPoint);
        int imr = 0;
        while (openList.CurrentItemCount > 0)
        {
            imr++;
            if (imr == 100000)
            {
                return null;
            }
            currentTile = openList.RemoveFirst();
            closedList.Add(currentTile);

            if (ReferenceEquals(currentTile, destenation))
            {
                return RetracePath(startingPoint, destenation);
            }

            foreach (TileSD neighbour in GameManager.Instance.GridBuilder.GetNeighbours(currentTile, map))
            {
                if (closedList.Contains(neighbour) || (neighbour.Occupied && !ReferenceEquals(neighbour, destenation)))
                {
                    continue;
                }

                int newNeighbourMovementCost = currentTile.costToStart + GetDistanceOfTiles(currentTile.Pos, neighbour.Pos);
                if (newNeighbourMovementCost < neighbour.costToStart || !openList.Contains(neighbour))
                {
                    neighbour.costToStart = newNeighbourMovementCost;
                    neighbour.costToEnd = GetDistanceOfTiles(neighbour.Pos, destenation.Pos);
                    neighbour.PathParent = currentTile;
                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                    else
                    {
                        openList.UpdateItem(neighbour);
                    }
                }
            }

        }
        return null;
    }


    private List<TileSD> RetracePath(TileSD start, TileSD end)
    {
        List<TileSD> path = new List<TileSD>();
        TileSD cur = end;
        while (!ReferenceEquals(cur, start))
        {
            if (cur.Pos == start.Pos)
            {
                break;
            }
            path.Add(cur);
            cur = cur.PathParent;
        }
        path.Reverse();
        return path;
    }


    public static int GetDistanceOfTiles(Vector2Int origin, Vector2Int destenation)
    {
        int distX = Mathf.Abs(destenation.x - origin.x);
        int distY = Mathf.Abs(destenation.y - origin.y);

        return distX + distY;
    }
}
