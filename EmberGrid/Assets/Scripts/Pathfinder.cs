using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinder
{
    private Heap<TileSD> openList;
    private List<TileSD> closedList;


    private List<TileSD> path = new List<TileSD>();

    [ContextMenu("find path")]
    public void AttempFindingPath()
    {
        //FindPathToDest(test.CurrentPos, GameManager.Instance.PlayerWrapper.PlayerMovement.CurrentTile);
    }

    public List<TileSD> FindPathToDest(TileSD startingPoint, TileSD destenation, Dictionary<Vector2Int, TileSD> map)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        openList = new Heap<TileSD>(map.Count);
        closedList = new List<TileSD>();
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
                //found path
                sw.Stop();
                return RetracePath(startingPoint, destenation);
            }

            foreach (TileSD neighbour in GameManager.Instance.GridBuilder.GetNeighbours(currentTile, map))
            {
                if (closedList.Contains(neighbour) || (neighbour.Occupied && !ReferenceEquals(neighbour, destenation)))
                {
                    //if the neighbour was already the current tile.
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
