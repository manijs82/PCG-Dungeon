using System.Collections.Generic;
using MeshGen;
using UnityEngine;

public abstract class EnvironmentMesh
{
    protected Material[] materials;
    protected HeightMap heightMap;
    
    public EnvironmentMesh(Material[] materials, HeightMap heightMap)
    {
        this.materials = materials;
        this.heightMap = heightMap;
    }

    protected float GetHeightAt(int x, int y)
    {
        return heightMap.heights[x, y];
    }
    
    public abstract void PlaceMeshes(Dungeon dungeon);
    
    protected abstract IEnumerable<Vector3> GetPositions();
    
    protected abstract Mesh[] GetMeshVariations();

    protected abstract MeshData GetRandomMeshData();
}