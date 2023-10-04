using Freya;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapVisual : DungeonVisualizer
{
    [SerializeField] private int scale = 1;
    [SerializeField] private TileSet dungeonSet;
    [SerializeField] private TileSet environmentSet;
    [SerializeField] private TileSet sideSet;
    [SerializeField] private TileSet sideTwoSet;
    [SerializeField] private TileSet hallwaySet;
    [SerializeField] private TileSet riverSet;

    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private float perlinSnapInterval = 0.2f;
    [SerializeField] private float perlinScale = 0.1f;
    
    private float perlinOffset;
    private Tilemap tilemap;
    
    protected override void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.orientationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        base.Awake();
    }

    protected override void Visualize(Dungeon dungeon)
    {
        perlinOffset = Generator.tileRnd.Next(0, 2000);
        for (int y = 0; y < dungeon.grid.Height * scale; y++)
        {
            for (int x = 0; x < dungeon.grid.Width * scale; x++)
            {
                var tile = (TileGridObject)dungeon.grid.GetValue(x / scale, y / scale);
                tilemap.SetTile(GetTileData(new Vector3Int(x, y), tile), true);
            }
        }
        //tilemap.RefreshAllTiles();
    }

    private TileChangeData GetTileData(Vector3Int pos, TileGridObject tile)
    {
        float noise = Mathf.PerlinNoise((tile.x + perlinOffset) * perlinScale, (tile.y + perlinOffset) * perlinScale);
        noise = Mathf.Round(noise / perlinSnapInterval) * perlinSnapInterval;
        
        TileChangeData data = new TileChangeData();
        data.position = pos;
        if (tile is RoomTileObject roomTile)
        {
            switch (roomTile.environmentType)
            {
                case EnvironmentType.Forest:
                    data.color = tile.Type == CellType.Wall ? Color.white : Color.Lerp(color1, color2, noise);
                    data.tile = environmentSet.GetTile(tile.Type);
                    break;
                case EnvironmentType.Room:
                    data = dungeonSet.GetTileData(tile.Type, pos);
                    break;
                case EnvironmentType.Set:
                    data = sideSet.GetTileData(tile.Type, pos);
                    break;
                case EnvironmentType.SetTwo:
                    data = sideTwoSet.GetTileData(tile.Type, pos);
                    break;
            }
        }
        else if (tile is RiverTileObject)
        {
            data = riverSet.GetTileData(tile.Type, pos);
        }
        else
        {
            data.tile = hallwaySet.GetTile(tile.Type);
            if (tile.Type == CellType.Empty)
            {
                data.color = Color.Lerp(color1, color2, noise);
            }
            else
            {
                data.color = Color.white;
            }
        }

        data.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        return data;
    }
}
