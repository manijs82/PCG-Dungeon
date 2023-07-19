using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapVisual : DungeonVisualizer
{
    [SerializeField] private int scale = 1;
    [SerializeField] private TileSet dungeonSet;
    [SerializeField] private TileSet environmentSet;
    [SerializeField] private TileSet hallwaySet;
    
    private Tilemap tilemap;
    
    protected override void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.orientationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        base.Awake();
    }

    protected override void Visualize(Dungeon dungeon)
    {
        for (int y = 0; y < dungeon.grid.Height * scale; y++)
        {
            for (int x = 0; x < dungeon.grid.Width * scale; x++)
            {
                var tile = (TileGridObject)dungeon.grid.GetValue(x / scale, y / scale);
                if (tile is RoomTileObject roomTile)
                {
                    tilemap.SetTile(new Vector3Int(x, y), 
                        roomTile.environmentType == EnvironmentType.Room ? dungeonSet.GetTile(tile.Type) : environmentSet.GetTile(tile.Type));    
                    continue;
                }
                tilemap.SetTile(new Vector3Int(x, y), hallwaySet.GetTile(tile.Type));
            }
        }
    }
}
