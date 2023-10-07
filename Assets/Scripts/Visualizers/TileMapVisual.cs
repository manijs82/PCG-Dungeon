using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapVisual : DungeonVisualizer
{
    [SerializeField] private bool animate;
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
        tilemap.ClearAllTiles();
        
        perlinOffset = Generator.tileRnd.Next(0, 2000);
        StartCoroutine(SetTiles(dungeon));
    }

    private IEnumerator SetTiles(Dungeon dungeon)
    {
        for (var i = 0; i < dungeon.grid.riverTiles.Count; i++)
        {
            var riverTile = dungeon.grid.riverTiles[i];
            
            if(i % 8 == 0 && animate)
                yield return null;

            tilemap.SetTile(riverSet.GetTileData(riverTile.Type, new Vector3Int(riverTile.x, riverTile.y)), true);
        }

        for (var i = 0; i < dungeon.grid.roomTiles.Count; i++)
        {
            var roomTile = dungeon.grid.roomTiles[i];
            
            if(i % 15 == 0 && animate)
                yield return null;

            float noise = Mathf.PerlinNoise((roomTile.x + perlinOffset) * perlinScale,
                (roomTile.y + perlinOffset) * perlinScale);
            noise = Mathf.Round(noise / perlinSnapInterval) * perlinSnapInterval;

            TileChangeData data = new TileChangeData();
            var pos = new Vector3Int(roomTile.x, roomTile.y);
            data.position = pos;
            switch (roomTile.environmentType)
            {
                case EnvironmentType.Forest:
                    data.color = roomTile.Type == CellType.Wall ? Color.white : Color.Lerp(color1, color2, noise);
                    data.tile = environmentSet.GetTile(roomTile.Type);
                    data.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
                    break;
                case EnvironmentType.Room:
                    data = dungeonSet.GetTileData(roomTile.Type, pos);
                    break;
                case EnvironmentType.Set:
                    data = sideSet.GetTileData(roomTile.Type, pos);
                    break;
                case EnvironmentType.SetTwo:
                    data = sideTwoSet.GetTileData(roomTile.Type, pos);
                    break;
            }

            tilemap.SetTile(data, true);
        }

        for (var i = 0; i < dungeon.grid.hallwayTiles.Count; i++)
        {
            var hallwayTile = dungeon.grid.hallwayTiles[i];
            
            if(i % 8 == 0 && animate)
                yield return null;

            if (!hallwayTile.isOverRiver)
                tilemap.SetTile(hallwaySet.GetTileData(hallwayTile.Type, new Vector3Int(hallwayTile.x, hallwayTile.y)),
                    true);
            else
                tilemap.SetTile(riverSet.GetTileData(hallwayTile.Type, new Vector3Int(hallwayTile.x, hallwayTile.y)),
                    true);
        }

        for (var i = 0; i < dungeon.grid.backgroundTiles.Count; i++)
        {
            var backTile = dungeon.grid.backgroundTiles[i];
            
            if(i % 50 == 0 && animate)
                yield return null;

            float noise = Mathf.PerlinNoise((backTile.x + perlinOffset) * perlinScale,
                (backTile.y + perlinOffset) * perlinScale);
            noise = Mathf.Round(noise / perlinSnapInterval) * perlinSnapInterval;

            var data = hallwaySet.GetTileData(backTile.Type, new Vector3Int(backTile.x, backTile.y));
            if (backTile.Type == CellType.Empty)
            {
                data.color = Color.Lerp(color1, color2, noise);
            }

            tilemap.SetTile(data, true);
        }

        yield return new WaitForEndOfFrame();
    }
}
