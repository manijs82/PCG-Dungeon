using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapVisual : DungeonVisualizer
{
    [SerializeField] private bool animate;
    [SerializeField] private bool placeNpcs;
    [SerializeField] private Npc npcPrefab;
    
    private Tilemap tilemap;
    
    protected override void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.orientationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        base.Awake();
    }

    protected override void Visualize(Dungeon dungeon)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        tilemap.ClearAllTiles();
        
        ServiceLocator.PerlinNoiseProvider.SetPerlinOffset(Generator.tileRnd.Next(0, 2000));
        
        if(animate)
            StartCoroutine(SetTilesAnimated(dungeon));
        else
        {
            foreach (var gridObject in dungeon.grid.GridObjects)
            {
                tilemap.SetTile(((TileGridObject)gridObject).GetTileVisual(), true);
            }
        }
        
        if(placeNpcs)
            NpcPlacer.Place(dungeon, npcPrefab);
        
        stopwatch.Stop();
        print($"Rendering time: '{stopwatch.ElapsedMilliseconds}'ms");
    }

    private IEnumerator SetTilesAnimated(Dungeon dungeon)
    {
        for (var i = 0; i < dungeon.grid.riverTiles.Count; i++)
        {
            var riverTile = dungeon.grid.riverTiles[i];
            
            if(i % riverTile.AnimateSpeed == 0)
                yield return new WaitForSeconds(0.008f);

            tilemap.SetTile(riverTile.GetTileVisual(), true);
        }

        for (var i = 0; i < dungeon.grid.roomTiles.Count; i++)
        {
            var roomTile = dungeon.grid.roomTiles[i];
            
            if(i % roomTile.AnimateSpeed == 0)
                yield return new WaitForSeconds(0.008f);

            tilemap.SetTile(roomTile.GetTileVisual(), true);
        }

        for (var i = 0; i < dungeon.grid.hallwayTiles.Count; i++)
        {
            var hallwayTile = dungeon.grid.hallwayTiles[i];
            
            if(i % hallwayTile.AnimateSpeed == 0)
                yield return new WaitForSeconds(0.008f);

            tilemap.SetTile(hallwayTile.GetTileVisual(), true);
        }

        for (var i = 0; i < dungeon.grid.backgroundTiles.Count; i++)
        {
            var backTile = dungeon.grid.backgroundTiles[i];
            
            if(i % backTile.AnimateSpeed == 0)
                yield return new WaitForSeconds(0.008f);

            tilemap.SetTile(backTile.GetTileVisual(), true);
        }

        yield return new WaitForEndOfFrame();
    }
}
