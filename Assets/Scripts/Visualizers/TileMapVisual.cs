using System;
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
                tilemap.SetTile(GetTileData(new Vector3Int(x, y), tile), true);
            }
        }
        tilemap.RefreshAllTiles();
    }

    private TileChangeData GetTileData(Vector3Int pos, TileGridObject tile)
    {
        TileChangeData data = new TileChangeData();
        data.position = pos;
        if (tile is RoomTileObject roomTile)
        {
            switch (roomTile.environmentType)
            {
                case EnvironmentType.Forest:
                    data.tile = environmentSet.GetTile(tile.Type);
                    break;
                case EnvironmentType.Room:
                    data.tile = dungeonSet.GetTile(tile.Type);
                    break;
            }
        }
        else
        {
            data.tile = hallwaySet.GetTile(tile.Type);
        }
        
        data.color = Color.white;

        data.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        return data;
    }
}
