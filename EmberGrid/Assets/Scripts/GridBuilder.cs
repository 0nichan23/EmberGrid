using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private CustomTile[] tiles;

    private void Start()
    {
        Application.targetFrameRate = 120;
        GenerateGrid();
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
                    Instantiate(tile.Prefab, tilemap.CellToWorld(tilePosition), Quaternion.identity).transform.SetParent(tilemap.transform);
                }

            }
        }
    }

}
