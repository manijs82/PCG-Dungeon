using System.Collections.Generic;
using System.Linq;
using MeshGen;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator
{
    public int maxSectionWidth = 80;

    private Dungeon dungeon;
    private NoiseMap noiseMap;
    private int width;
    private MeshDataOld[] meshSections;
    
    public TerrainGenerator(Dungeon dungeon, NoiseMap noiseMap)
    {
        this.dungeon = dungeon;
        this.noiseMap = noiseMap;
        width = dungeon.dungeonParameters.width;
    }

    public Mesh[] GenerateTerrainSections()
    {
        int sectionWidth = Mathf.CeilToInt(dungeon.grid.Width / (float)maxSectionWidth);
        int sectionHeight = Mathf.CeilToInt(dungeon.grid.Height / (float)maxSectionWidth);
        meshSections = new MeshDataOld[sectionWidth * sectionHeight];

        for (int x = 0, i = 0; x < sectionWidth; x++)
        {
            for (int y = 0; y < sectionHeight; y++, i++)
            {
                MeshDataOld meshData = new MeshDataOld();
                int squareIndex = 0;

                var gridObjects = dungeon.grid.GetSection(x, y, maxSectionWidth, maxSectionWidth);
                foreach (var tile in gridObjects)
                {
                    AddTileQuadToMesh(meshData, tile, squareIndex);
                    squareIndex += 4;
                }

                meshSections[i] = meshData;
            }
        }

        Mesh[] meshes = new Mesh[meshSections.Length];
        for (var i = 0; i < meshes.Length; i++) 
            meshes[i] = meshSections[i].CreateMesh(false);

        return meshes;
    }

    private void AddTileQuadToMesh(MeshDataOld meshData, GridObject tile, int squareIndex)
    {
        meshData.AddVertex(new Vector3(tile.x, noiseMap[tile.x, tile.y], tile.y));
        meshData.AddVertex(new Vector3(tile.x, noiseMap[tile.x, tile.y + 1], tile.y + 1));
        meshData.AddVertex(new Vector3(tile.x + 1, noiseMap[tile.x + 1, tile.y + 1], tile.y + 1));
        meshData.AddVertex(new Vector3(tile.x + 1, noiseMap[tile.x + 1, tile.y], tile.y));

        meshData.AddTriangle(squareIndex, squareIndex + 1, squareIndex + 2);
        meshData.AddTriangle(squareIndex + 2, squareIndex + 3, squareIndex);

        var normal = noiseMap.GetNormalAt(tile.x + 0.5f, tile.y + 0.5f, 0.5f);
        meshData.AddNormal(normal);
        meshData.AddNormal(normal);
        meshData.AddNormal(normal);
        meshData.AddNormal(normal);
        
        ServiceLocator.dungeonShapesDrawer.AddShape(() =>
        {
            var start = new Vector3(tile.x + 0.5f, noiseMap.GetValueAt(tile.x + 0.5f, tile.y + 0.5f), tile.y + 0.5f);
            Gizmos.DrawLine(start, start + normal);
        }, "3D_Terrain");
    }
}