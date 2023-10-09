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
        
        ServiceLocator.PerlinNoiseProvider.SetPerlinOffset(Generator.tileRnd.Next(0, 2000));
        StartCoroutine(SetTiles(dungeon));
    }

    private IEnumerator SetTiles(Dungeon dungeon)
    {
        for (var i = 0; i < dungeon.grid.riverTiles.Count; i++)
        {
            var riverTile = dungeon.grid.riverTiles[i];
            
            if(i % 8 == 0 && animate)
                yield return null;

            tilemap.SetTile(riverTile.GetTileVisual(), true);
        }

        for (var i = 0; i < dungeon.grid.roomTiles.Count; i++)
        {
            var roomTile = dungeon.grid.roomTiles[i];
            
            if(i % 15 == 0 && animate)
                yield return null;

            tilemap.SetTile(roomTile.GetTileVisual(), true);
        }

        for (var i = 0; i < dungeon.grid.hallwayTiles.Count; i++)
        {
            var hallwayTile = dungeon.grid.hallwayTiles[i];
            
            if(i % 8 == 0 && animate)
                yield return null;

            tilemap.SetTile(hallwayTile.GetTileVisual(), true);
        }

        for (var i = 0; i < dungeon.grid.backgroundTiles.Count; i++)
        {
            var backTile = dungeon.grid.backgroundTiles[i];
            
            if(i % 50 == 0 && animate)
                yield return null;

            tilemap.SetTile(backTile.GetTileVisual(), true);
        }

        yield return new WaitForEndOfFrame();
    }
}
