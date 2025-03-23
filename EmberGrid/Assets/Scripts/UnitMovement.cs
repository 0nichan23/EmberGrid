using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitMovement 
{
    [SerializeField] private TileSD currentTile;
    public TileSD CurrentTile { get => currentTile; set => currentTile = value; }
}
