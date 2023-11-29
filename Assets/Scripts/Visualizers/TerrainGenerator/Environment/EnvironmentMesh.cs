using System.Collections.Generic;
using MeshGen;
using UnityEngine;

public abstract class EnvironmentMesh
{
    protected Material[] materials;
    
    public EnvironmentMesh(Material[] materials)
    {
        this.materials = materials;
    }
    
    public abstract void PlaceMeshes(Dungeon dungeon);
    
    protected abstract List<Vector3> GetPositions();
    
    protected abstract Mesh[] GetMeshVariations();

    protected abstract MeshData GetRandomMeshData();
}