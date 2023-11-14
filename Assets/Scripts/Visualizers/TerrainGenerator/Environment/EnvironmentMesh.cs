using System.Collections.Generic;
using MeshGen;
using UnityEngine;

public abstract class EnvironmentMesh
{
    protected Material material;
    
    public EnvironmentMesh(Material material)
    {
        this.material = material;
    }
    
    public abstract void PlaceMeshes(Dungeon dungeon);
    
    protected abstract List<Vector3> GetPositions();
    
    protected abstract Mesh[] GetMeshVariations();

    protected abstract MeshData GetRandomMeshData();
}