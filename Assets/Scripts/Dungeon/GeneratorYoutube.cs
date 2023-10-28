using System.Collections;
using System.Collections.Generic;
using Mani;
using Mani.Graph;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneratorYoutube : MonoBehaviour
{
    
    [SerializeField] private DungeonParameters dungeonParameters;
    [SerializeField] private Tilemap tilemap;
    [Space]
    [SerializeField] private bool newDungeon;
    [SerializeField] private bool mutate;
    [SerializeField] private bool triangulate;
    [SerializeField] private bool mst;
    [SerializeField] private bool doAll;
    [SerializeField] private bool drawRoom;
    [SerializeField] private bool drawHallway;
    [SerializeField] private bool drawBackground;
    [SerializeField] private bool clear;
    
    private Dungeon dungeon;
    
    private void Start()
    {
        //dungeon = new Dungeon(dungeonParameters);
    }

    #if UNITY_EDITOR
    
    private void OnDrawGizmos()
    {
        if (newDungeon)
        {
            dungeon = new Dungeon(dungeonParameters);
            newDungeon = false;
        }
        
        if (dungeon == null)
            return;
        
        var matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Handles.matrix = matrix;
        
        
        foreach (var room in dungeon.rooms)
        {
            DrawOutline(room.bound);
        }

        if (dungeon.roomGraph != null)
        {
            Handles.color = Color.black;
            
            foreach (var connection in dungeon.roomGraph.Edges)
            {
                Handles.DrawAAPolyLine(connection.Start.Value.Center, connection.End.Value.Center);
            }
        }

        if (doAll)
        {
            for (int i = 0; i < 100; i++)
            {
                dungeon.Mutate();
            }
            
            dungeon.roomGraph = new Graph<Room>();
            foreach (var room in dungeon.rooms) dungeon.roomGraph.AddNode(new Node<Room>(room));
            dungeon.roomGraph.TriangulateDelaunay(node => node.Value.Center.ToPoint());
            dungeon.roomGraph = dungeon.roomGraph.GetPrimsMinimumSpanningTree(true, 0.2f, dungeonParameters.width / 6);
            doAll = false;
        }

        if (mutate)
        {
            dungeon.Mutate();

            mutate = false;
        }

        if (triangulate)
        {
            dungeon.roomGraph = new Graph<Room>();
            foreach (var room in dungeon.rooms) dungeon.roomGraph.AddNode(new Node<Room>(room));
            dungeon.roomGraph.TriangulateDelaunay(node => node.Value.Center.ToPoint());
            triangulate = false;
        }

        if (mst && dungeon.roomGraph != null)
        {
            dungeon.roomGraph = dungeon.roomGraph.GetPrimsMinimumSpanningTree(true, 0.2f, dungeonParameters.width / 6);
            mst = false;
        }

        if (drawRoom && dungeon.roomGraph != null)
        {
            dungeon.MakeGrid();
            tilemap.ClearAllTiles();
            tilemap.orientationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

            foreach (var node in dungeon.roomGraph.Nodes)
            {
                StartCoroutine(SetRoomTiles(node.Value));
            }

            drawRoom = false;
        }
        
        if (drawHallway && dungeon.roomGraph != null)
        {
            StartCoroutine(SetHallwayTiles(dungeon.grid.hallwayTiles));

            drawHallway = false;
        }
        
        if (drawBackground && dungeon.roomGraph != null)
        {
            StartCoroutine(SetTiles(dungeon.grid.backgroundTiles));

            drawBackground = false;
        }
        
        if (clear)
        {
            tilemap.ClearAllTiles();

            clear = false;
        }
        
        Handles.matrix = Matrix4x4.identity;
    }

    private IEnumerator SetRoomTiles(Room room)
    {
        var width = room.grid.GridObjects.GetLength(0) - 1;
        var height = room.grid.GridObjects.GetLength(1) - 1;
        for (var x = 0; x <= Mathf.FloorToInt(width / 2f); x++)
        {
            for (var y = 0; y <= Mathf.FloorToInt(height / 2f); y++)
            {
                var gridObject1 = room.grid.GridObjects[x, y];
                var gridObject2 = room.grid.GridObjects[width - x, height - y];
                var gridObject3 = room.grid.GridObjects[width - x, y];
                var gridObject4 = room.grid.GridObjects[x, height - y];
                yield return new WaitForSeconds(0.016f);
                tilemap.SetTile(((TileGridObject)gridObject1).GetTileVisual(), true);
                tilemap.SetTile(((TileGridObject)gridObject2).GetTileVisual(), true);
                tilemap.SetTile(((TileGridObject)gridObject3).GetTileVisual(), true);
                tilemap.SetTile(((TileGridObject)gridObject4).GetTileVisual(), true);
            }
        }
    }
    
    private IEnumerator SetHallwayTiles(List<HallwayTileObject> tiles)
    {
        for (var i = 0; i < tiles.Count; i++)
        {
            var hallwayTile = tiles[i];
            
            if(i % hallwayTile.AnimateSpeed == 0)
                yield return new WaitForSeconds(0.016f);

            tilemap.SetTile(hallwayTile.GetTileVisual(), true);
        }
    }
    
    private IEnumerator SetTiles(List<TileGridObject> tiles)
    {
        for (var i = 0; i < tiles.Count; i++)
        {
            var hallwayTile = tiles[i];
            
            if(i % hallwayTile.AnimateSpeed == 0)
                yield return new WaitForSeconds(0.016f);

            tilemap.SetTile(hallwayTile.GetTileVisual(), true);
        }
    }

    private void DrawOutline(Bound bound)
    {
        Handles.color = Color.white;
        Handles.DrawAAPolyLine(new Vector3(bound.x, bound.y), new Vector3(bound.XPW, bound.y));
        Handles.DrawAAPolyLine(new Vector3(bound.x, bound.y), new Vector3(bound.x, bound.YPH));
        Handles.DrawAAPolyLine(new Vector3(bound.XPW, bound.y), new Vector3(bound.XPW, bound.YPH));
        Handles.DrawAAPolyLine(new Vector3(bound.x, bound.YPH), new Vector3(bound.XPW, bound.YPH));
        Handles.DrawSolidDisc(bound.Center, Vector3.back, 1f);
    }
    
    #endif
}