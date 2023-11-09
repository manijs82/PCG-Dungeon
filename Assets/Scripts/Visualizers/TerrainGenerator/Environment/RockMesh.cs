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
        
        meshData.AddVertex(0, 0, 0);
        meshData.AddVertex(0, 1, 0);
        meshData.AddVertex(1, 0, 0);
        meshData.AddVertex(1, 1, 0);
        //meshData.AddVertex(-0.5f, 0.5f, 0);
        //meshData.AddVertex(0.5f, -0.5f, 0);
        //meshData.AddVertex(1.5f, -0.5f, 0);
        //meshData.AddVertex(0, -0.5f, 0);

        var tri1 = meshData.AddTriangle(0, 1, 2);
        //meshData.AddTriangle(1, 3, 2);
        //meshData.AddTriangle(0, 4, 1);
        //meshData.AddTriangle(2, 5, 0);
        //meshData.AddTriangle(2, 6, 5);
        //meshData.AddTriangle(0, 5, 7);
        //meshData.AddTriangle(4, 0, 7);
        //meshData.AddTriangle(2, 3, 6);

        meshData.RotateTriangle(tri1, Vector3.forward, 90);
        
        return meshData;
    }
}