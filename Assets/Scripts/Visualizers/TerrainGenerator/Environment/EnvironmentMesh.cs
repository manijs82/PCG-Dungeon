using System.Collections.Generic;
using MeshGen;
using UnityEngine;

public abstract class EnvironmentMesh
{
    protected Material[] materials;
    protected HeightMap heightMap;
    protected List<Vector3> positionSamples;
    
    public EnvironmentMesh(Material[] materials, HeightMap heightMap, List<Vector3> positionSamples)
    {
        this.materials = materials;
        this.heightMap = heightMap;
        this.positionSamples = positionSamples;
    }

    protected float GetHeightAt(int x, int y)
    {
        return heightMap[x, y];
    }
    
    protected float GetHeightAt(float x, float y)
    {
        return heightMap.GetHeightAt(x, y);
    }
    
    public abstract void PlaceMeshes(Dungeon dungeon);
    
    protected abstract IEnumerable<Vector3> GetPositions();
    
    protected abstract Mesh[] GetMeshVariations();

    protected abstract MeshData GetRandomMeshData();
}