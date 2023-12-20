using System.Collections.Generic;
using MeshGen;
using UnityEngine;

public class TreeMesh : EnvironmentMesh
{
    private Dungeon dungeon;
    
    public TreeMesh(Material[] materials, HeightMap heightMap) : base(materials, heightMap)
    {
    }

    public override void PlaceMeshes(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        
        var meshVariation = GetMeshVariations();
        
        foreach (var position in GetPositions())
        {
            var go = new GameObject("Tree");
            go.transform.position = new Vector3(position.x, GetHeightAt((int) position.x, (int) position.y), position.y);
        
            go.AddComponent<MeshFilter>().mesh = meshVariation[Random.Range(0, meshVariation.Length)];
            go.AddComponent<MeshRenderer>().materials = materials;
        }
    }

    protected override IEnumerable<Vector3> GetPositions()
    {
        PerlinMask perlinMask = new PerlinMask(dungeon, 0.5f);
        BackgroundMask backgroundMask = new BackgroundMask(dungeon);
        SurroundingRoomMask surroundingRoomMask = new SurroundingRoomMask(dungeon, EnvironmentType.Set, true);
        var mask = Mask.GetCombinedMask(CombineMode.Intersection, perlinMask, backgroundMask, surroundingRoomMask);
        
        return PoissonDiscSampling.GeneratePoints(7, dungeon.bound, 1000).MaskPositions(mask);
    }

    protected override Mesh[] GetMeshVariations()
    {
        Mesh[] meshes = new Mesh[10];
        for (int i = 0; i < meshes.Length; i++)
        {
            var randomMeshData = GetRandomMeshData();
            var mesh = randomMeshData.CreateMesh(true, true);
            mesh.name = $"Tree {i}";

            meshes[i] = mesh;
        }
        
        return meshes;
    }

    protected override MeshData GetRandomMeshData()
    {
        MeshData meshData = new MeshData();

        float height = Random.Range(5, 7);

        meshData.AddRoundedCube(Vector3.up * height, new Vector3(height - 1, height - Random.Range(1, 2), height - 1), Random.Range(2, 5));
        meshData.subMeshes.Add(meshData.triangles.Count - 1);
        meshData.AddCube(Vector3.down, new Vector3(1, height + 1, 1));
        meshData.subMeshes.Add(meshData.triangles.Count - 1);
        
        meshData.ScaleMesh(Vector3.one * 0.5f);

        return meshData;
    }
}