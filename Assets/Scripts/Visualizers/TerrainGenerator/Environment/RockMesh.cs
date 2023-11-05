using MeshGen;
using UnityEngine;

public class RockMesh : EnvironmentMesh
{
    public MeshData mesh;
    
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
        
        meshData.AddTriangle(Vector2.zero, Vector2.up, Vector2.right);
        meshData.AddConnectedTriangle(Vector2.one, 0, 1);
        meshData.AddConnectedTriangle(Vector3.one, 1, 0);
        meshData.AddConnectedTriangle(new Vector3(1, 0, 1), 0, 2);
        meshData.AddConnectedTriangle(new Vector3(0, 1, 1), 0, 0);
        meshData.AddConnectedTriangle(new Vector3(1, 0, 1), 1, 1);

        mesh = meshData;
        return meshData;
    }
}