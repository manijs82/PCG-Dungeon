using MeshGen;
using UnityEngine;
using Utils;

public class RockMesh : EnvironmentMesh
{
    public RockMesh(Material material) : base(material)
    {
    }

    public override void PlaceMeshes(Dungeon dungeon)
    {
        var go = new GameObject("Cube");
        var mesh = GetRandomMeshData().CreateMesh();
        mesh.RecalculateNormals();
        
        go.AddComponent<MeshFilter>().mesh = mesh;
        go.AddComponent<MeshRenderer>().material = material;
    }

    protected override Vector3[] GetPositions()
    {
        return new Vector3[] { };
    }

    protected override Mesh[] GetMeshVariations()
    {
        return new Mesh[] { };
    }

    protected override MeshData GetRandomMeshData()
    {
        MeshData meshData = new MeshData();
        
        meshData.AddCube(Vector3.zero, new Vector3(1, 2, 3));
        
        return meshData;
    }
}