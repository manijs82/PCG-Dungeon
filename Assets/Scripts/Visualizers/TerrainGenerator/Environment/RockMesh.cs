using MeshGen;
using UnityEngine;

public class RockMesh : EnvironmentMesh
{
    public RockMesh(Material material) : base(material)
    {
    }

    public override void PlaceMeshes(Dungeon dungeon)
    {
        var go = new GameObject("Cube");
        go.transform.position = GetPositions()[0];
        var randomMeshData = GetRandomMeshData();
        var mesh = randomMeshData.CreateMesh();
        mesh.RecalculateNormals();
        
        go.AddComponent<MeshFilter>().mesh = mesh;
        go.AddComponent<MeshRenderer>().material = material;
        go.AddComponent<MeshGizmo>().SetMeshData(randomMeshData);
    }

    protected override Vector3[] GetPositions()
    {
        return new Vector3[] { new Vector3(-13, 0, 0) };
    }

    protected override Mesh[] GetMeshVariations()
    {
        return new Mesh[] { };
    }

    protected override MeshData GetRandomMeshData()
    {
        MeshData meshData = new MeshData();
        
        //meshData.AddCube(Vector3.zero, Vector3.one / 2f);
        meshData.AddQuad(Vector3.zero, Vector3.right / 2f, Vector3.up / 2f);
        meshData.RemoveTriangle(1);
        //meshData.InsertVertexTriangle(0);
        
        return meshData;
    }
}