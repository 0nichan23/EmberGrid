using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Custom Tile")]
public class CustomTile : TileBase
{

    [SerializeField] private TilePrefab prefab;

    public TilePrefab Prefab { get => prefab;}

 

}
