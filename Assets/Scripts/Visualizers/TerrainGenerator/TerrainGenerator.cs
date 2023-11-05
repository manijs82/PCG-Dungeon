using MeshGen;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator
{
    public int lod = 0;

    private Dungeon dungeon;
    private int width;
    private MeshData meshData;
    
    public TerrainGenerator(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        width = dungeon.dungeonParameters.width;
        meshData = new MeshData();
    }

    public Mesh GenerateMesh()
    {
        int squareIndex = 0;

        foreach (var tile in dungeon.grid.backgroundTiles)
        {
            AddTileQuadToMesh(tile, squareIndex);

            squareIndex += 4;
        }
        
        return meshData.CreateMesh();
    }

    private void AddTileQuadToMesh(TileGridObject tile, int squareIndex)
    {
        meshData.AddVertex(new Vector3(tile.x, 0, tile.y));
        meshData.AddVertex(new Vector3(tile.x, 0, tile.y + 1));
        meshData.AddVertex(new Vector3(tile.x + 1, 0, tile.y + 1));
        meshData.AddVertex(new Vector3(tile.x + 1, 0, tile.y));

        meshData.AddTriangle(squareIndex, squareIndex + 1, squareIndex + 2);
        meshData.AddTriangle(squareIndex + 2, squareIndex + 3, squareIndex);

        var tileSprite = ((Tile)tile.GetTileVisual().tile).sprite;
        meshData.AddUV(tileSprite.uv[2]);
        meshData.AddUV(tileSprite.uv[0]);
        meshData.AddUV(tileSprite.uv[1]);
        meshData.AddUV(tileSprite.uv[3]);
    }
}