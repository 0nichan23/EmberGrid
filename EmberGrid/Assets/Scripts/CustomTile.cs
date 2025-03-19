using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Custom Tile")]
public class CustomTile : TileBase
{

    [SerializeField] private Sprite tileSprite;
    [SerializeField] private TilePrefab prefab;

    public TilePrefab Prefab { get => prefab;}

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = tileSprite;
        tileData.flags = TileFlags.LockTransform;
    }

}
