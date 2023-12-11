using System.Collections.Generic;
using MeshGen;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator
{
    public int maxSectionWidth = 30;

    private Dungeon dungeon;
    private HeightMap heightMap;
    private int width;
    private MeshDataOld[] meshSections;
    
    public TerrainGenerator(Dungeon dungeon, HeightMap heightMap)
    {
        this.dungeon = dungeon;
        this.heightMap = heightMap;
        width = dungeon.dungeonParameters.width;
    }

    public Mesh[] GenerateMesh()
    {
        int sectionWidth = Mathf.CeilToInt(dungeon.grid.Width / (float)maxSectionWidth);
        int sectionHeight = Mathf.CeilToInt(dungeon.grid.Height / (float)maxSectionWidth);
        meshSections = new MeshDataOld[sectionWidth * sectionHeight];
        
        
        int squareIndex = 0;

        var gridObjects = dungeon.grid.GetAll();
        
        foreach (var tile in gridObjects)
        {
            AddTileQuadToMesh(null, tile, squareIndex);

            squareIndex += 4;
        }


        Mesh[] meshes = new Mesh[meshSections.Length];
        for (var i = 0; i < meshes.Length; i++) meshes[i] = meshSections[i].CreateMesh();

        return meshes;
    }

    private void AddTileQuadToMesh(MeshDataOld meshData, GridObject tile, int squareIndex)
    {
        meshData.AddVertex(new Vector3(tile.x, heightMap.heights[tile.x, tile.y], tile.y));
        meshData.AddVertex(new Vector3(tile.x, heightMap.heights[tile.x, tile.y + 1], tile.y + 1));
        meshData.AddVertex(new Vector3(tile.x + 1, heightMap.heights[tile.x + 1, tile.y + 1], tile.y + 1));
        meshData.AddVertex(new Vector3(tile.x + 1, heightMap.heights[tile.x + 1, tile.y], tile.y));

        meshData.AddTriangle(squareIndex, squareIndex + 1, squareIndex + 2);
        meshData.AddTriangle(squareIndex + 2, squareIndex + 3, squareIndex);

        //var tileSprite = ((Tile)tile.GetTileVisual().tile).sprite;
        //meshData.AddUV(tileSprite.uv[2]);
        //meshData.AddUV(tileSprite.uv[0]);
        //meshData.AddUV(tileSprite.uv[1]);
        //meshData.AddUV(tileSprite.uv[3]);
    }
}