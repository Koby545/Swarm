using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase grassTile;
    public TileBase dirtTile;

    public int mapWidth = 30;
    public int mapHeight = 20;
    public float dirtChance = 0.3f;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        tilemap.ClearAllTiles();

        for (int x = -mapWidth / 2; x < mapWidth / 2; x++)
        {
            for (int y = -mapHeight / 2; y < mapHeight / 2; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                // רנדומלי אבל עם Perlin Noise כדי שייראה טבעי
                float noise = Mathf.PerlinNoise(
                    (x + Random.Range(0, 1000)) * 0.3f,
                    (y + Random.Range(0, 1000)) * 0.3f
                );

                if (noise < dirtChance)
                    tilemap.SetTile(pos, dirtTile);
                else
                    tilemap.SetTile(pos, grassTile);
            }
        }
    }
}