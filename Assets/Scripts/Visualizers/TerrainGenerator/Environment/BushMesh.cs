using System.Collections.Generic;
using MeshGen;
using UnityEngine;

public class BushMesh : EnvironmentMesh
{
    public BushMesh(Material[] materials) : base(materials)
    {
    }

    public override void PlaceMeshes(Dungeon dungeon)
    {
        foreach (var position in GetPositions())
        {
            var go = new GameObject("Cube");
            go.transform.position = position;
            var randomMeshData = GetRandomMeshData();
            var mesh = randomMeshData.CreateMesh();
            mesh.RecalculateNormals();
        
            go.AddComponent<MeshFilter>().mesh = mesh;
            go.AddComponent<MeshRenderer>().materials = materials;
            go.AddComponent<MeshGizmo>().SetMeshData(randomMeshData);
        }
    }

    protected override List<Vector3> GetPositions()
    {
        return new List<Vector3>()
        {
            new Vector3(-13, 0, 0)
        };
    }

    protected override Mesh[] GetMeshVariations()
    {
        return new Mesh[] { };
    }

    protected override MeshData GetRandomMeshData()
    {
        MeshData meshData = new MeshData();
        
        //meshData.AddSubdividedCube(Vector3.zero, Vector3.one / 2f, 4, 4, 4);
        //meshData.AddSubdividedCube(Vector3.up * 2, Vector3.one / 2f, 2, 2, 2);
        //meshData.AddSubdividedCube(Vector2.one * 2, Vector3.one / 2f, 6, 2, 2);
        meshData.AddRoundedCube(Vector3.zero, new Vector3(4, 2, 4), 4);
        meshData.subMeshes.Add(meshData.triangles.Count - 1);
        meshData.AddCube(Vector3.down * 5, new Vector3(1, 5, 1));
        meshData.subMeshes.Add(meshData.triangles.Count - 1);

        return meshData;
    }
}