using System.Collections.Generic;
using MeshGen;
using UnityEngine;

public abstract class EnvironmentMesh
{
    protected Material[] materials;
    protected NoiseMap noiseMap;
    protected List<Vector3> positionSamples;
    
    public EnvironmentMesh(Material[] materials, NoiseMap noiseMap, List<Vector3> positionSamples)
    {
        this.materials = materials;
        this.noiseMap = noiseMap;
        this.positionSamples = positionSamples;
    }

    protected float GetHeightAt(int x, int y)
    {
        return noiseMap[x, y];
    }
    
    protected float GetHeightAt(float x, float y)
    {
        return noiseMap.GetValueAt(x, y);
    }
    
    public abstract void PlaceMeshes(Dungeon dungeon);
    
    protected abstract IEnumerable<Vector3> GetPositions();
    
    protected abstract Mesh[] GetMeshVariations();

    protected abstract MeshData GetRandomMeshData();
}