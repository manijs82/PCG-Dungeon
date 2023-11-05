using UnityEngine;

public abstract class EnvironmentMesh
{
    public abstract void PlaceMeshes(Dungeon dungeon);
    
    protected abstract Vector3[] GetPositions();
    
    protected abstract Mesh[] GetMeshVariations();
}