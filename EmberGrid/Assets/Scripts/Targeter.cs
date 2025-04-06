using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter
{
    public List<TileSD> TargetTilesRelativeToSource(UnitAction action, Direction direction, Vector2Int originPos)
    {
        List<TileSD> foundTiles = new List<TileSD>();   

        foreach (var item in action.Targets)
        {
            Vector2Int newPos = new Vector2Int();
            switch (direction)
            {
                case Direction.Up:
                    newPos = new Vector2Int(item.y * -1, item.x);
                    newPos += originPos;
                    break;
                case Direction.Down:
                    newPos = new Vector2Int(item.y, item.x * -1);
                    newPos += originPos;
                    break;
                case Direction.Left:
                    newPos = originPos - item;
                    break;
                case Direction.Right:
                    newPos = originPos + item;
                    break;
            }
            TileSD tile = GameManager.Instance.GridBuilder.GetTileFromPosition(newPos);
            if (!ReferenceEquals(tile,null))
            {
                foundTiles.Add(tile);
            }
        }
        return foundTiles;
    }

    public Direction GetCursorDirection(Unit unit)
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursorOffset = worldMousePos - unit.transform.position;

        // Ignore Z
        float absX = Mathf.Abs(cursorOffset.x);
        float absY = Mathf.Abs(cursorOffset.y);

        if (absX > absY)
        {
            return cursorOffset.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return cursorOffset.y > 0 ? Direction.Up : Direction.Down;
        }
    }

}

public enum Direction
{
    Up,
    Down, 
    Left, 
    Right
}
