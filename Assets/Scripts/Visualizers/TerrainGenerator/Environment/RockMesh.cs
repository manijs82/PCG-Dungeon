using System.Collections.Generic;
using MeshGen;
using UnityEngine;

public class RockMesh : EnvironmentMesh
{
    private Dungeon dungeon;
    
    public RockMesh(Material material) : base(material)
    {
    }

    public override void PlaceMeshes(Dungeon dungeon)
    {
        this.dungeon = dungeon;

        foreach (var position in GetPositions())
        {
            var go = new GameObject("Cube");
            go.transform.position = position;
            var randomMeshData = GetRandomMeshData();
            var mesh = randomMeshData.CreateMesh();
            mesh.RecalculateNormals();
        
            go.AddComponent<MeshFilter>().mesh = mesh;
            go.AddComponent<MeshRenderer>().material = material;
            go.AddComponent<MeshGizmo>().SetMeshData(randomMeshData);
        }
    }

    protected override List<Vector3> GetPositions()
    {
        var positions = new List<Vector3>();

        var mask = new SurroundingRoomMask(dungeon, EnvironmentType.Set).GetMask();
        
        for (int x = 0; x < mask.GetLength(0); x++)
        {
            for (int y = 0; y < mask.GetLength(1); y++)
            {
                if (mask[x, y])
                {
                    positions.Add(new Vector3(x, 0, y));
                }
            }
        }
        
        return positions;
    }

    protected override Mesh[] GetMeshVariations()
    {
        return new Mesh[] { };
    }

    protected override MeshData GetRandomMeshData()
    {
        MeshData meshData = new MeshData();
        
        meshData.AddCube(Vector3.zero, Vector3.one / 2f);
        var vIndex = meshData.InsertVertexQuad(10, 11);
        meshData.MoveVertex(vIndex, Vector3.up / 2f);

        for (var i = 0; i < meshData.vertices.Count; i++)
        {
            meshData.MoveVertex(i, Random.insideUnitSphere * 0.2f);
        }
        for (var i = 0; i < meshData.triangles.Count; i++)
        {
            var triangle = meshData.triangles[i];
            
            meshData.ScaleTriangle(triangle.index, Vector3.one * Random.Range(0.9f, 1.1f));
            meshData.RotateTriangle(triangle.index, Vector3.back, Random.Range(-5f, 5f));
        }
        
        meshData.RotateMesh(Vector3.up, Random.value * 360);
        meshData.RotateMesh(Vector3.right, Random.value * 360);
        meshData.RotateMesh(Vector3.forward, Random.value * 360);
        meshData.ScaleMesh(Vector3.one *  Random.Range(0.6f, 1.1f));

        return meshData;
    }
}