using System.Collections.Generic;
using MeshGen;
using UnityEngine;
using Utils;

public class RockMesh : EnvironmentMesh
{
    private Dungeon dungeon;
    
    public RockMesh(Material[] materials, HeightMap heightMap, List<Vector3> positionSamples) : base(materials, heightMap, positionSamples)
    {
    }

    public override void PlaceMeshes(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        var meshVariation = GetMeshVariations();

        foreach (var position in GetPositions())
        {
            var go = new GameObject("Rock");
            go.transform.position = new Vector3(position.x, GetHeightAt((int) position.x, (int) position.y), position.y);
            
            var mesh = meshVariation[Random.Range(0, meshVariation.Length)];
        
            go.AddComponent<MeshFilter>().mesh = mesh;
            go.AddComponent<MeshRenderer>().materials = materials;
        }
    }

    protected override IEnumerable<Vector3> GetPositions()
    {
        PerlinMask perlinMask = new PerlinMask(dungeon, 0.6f);
        BackgroundMask backgroundMask = new BackgroundMask(dungeon);
        var mask = Mask.GetCombinedMask(CombineMode.Intersection, perlinMask, backgroundMask);

        var maskedPositions = PoissonDiscSampling.GeneratePoints(positionSamples, 3, dungeon.bound, 2000).MaskPositions(mask);
        positionSamples.RemoveValues(maskedPositions);
        return maskedPositions;
    }

    protected override Mesh[] GetMeshVariations()
    {
        Mesh[] meshes = new Mesh[15];
        for (int i = 0; i < meshes.Length; i++)
        {
            var randomMeshData = GetRandomMeshData();
            var mesh = randomMeshData.CreateMesh(true, true);
            mesh.name = $"Rock {i}";

            meshes[i] = mesh;
        }
        
        return meshes;
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
        meshData.ScaleMesh(Vector3.one *  Random.Range(0.6f, 2.5f));

        return meshData;
    }
}