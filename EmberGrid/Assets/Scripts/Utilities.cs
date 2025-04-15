using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static Direction GetRotLeftDir(Direction current)
    {
        switch (current)
        {
            case Direction.Up:
                return Direction.Left;
            case Direction.Down:
                return Direction.Right;
            case Direction.Left:
                return Direction.Down;
            case Direction.Right:
                return Direction.Up;
        }
        return Direction.Right;
    }

    public static Direction GetRotRightDir(Direction current)
    {
        switch (current)
        {
            case Direction.Up:
                return Direction.Right;
            case Direction.Down:
                return Direction.Left;
            case Direction.Left:
                return Direction.Up;
            case Direction.Right:
                return Direction.Down;
        }
        return Direction.Right;
    }


}
