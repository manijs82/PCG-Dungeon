using System.Collections.Generic;
using MeshGen;
using UnityEngine;

public class BushMesh : EnvironmentMesh
{
    private Dungeon dungeon;
    
    public BushMesh(Material[] materials) : base(materials)
    {
    }

    public override void PlaceMeshes(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        
        var randomMeshData = GetRandomMeshData();
        var mesh = randomMeshData.CreateMesh();
        
        foreach (var position in GetPositions())
        {
            var go = new GameObject("Cube");
            go.transform.position = new Vector3(position.x, 0, position.y);
        
            go.AddComponent<MeshFilter>().mesh = mesh;
            go.AddComponent<MeshRenderer>().materials = materials;
            go.AddComponent<MeshGizmo>().SetMeshData(randomMeshData);
        }
    }

    protected override IEnumerable<Vector3> GetPositions()
    {
        PerlinMask perlinMask = new PerlinMask(dungeon, 0.5f);
        BackgroundMask backgroundMask = new BackgroundMask(dungeon);
        SurroundingRoomMask surroundingRoomMask = new SurroundingRoomMask(dungeon, EnvironmentType.Set, true);
        var mask = Mask.GetCombinedMask(CombineMode.Intersection, perlinMask, backgroundMask, surroundingRoomMask);
        
        return PoissonDiscSampling.GeneratePoints(2, dungeon.bound, 10000).MaskPositions(mask);
    }

    protected override Mesh[] GetMeshVariations()
    {
        return new Mesh[] { };
    }

    protected override MeshData GetRandomMeshData()
    {
        MeshData meshData = new MeshData();

        meshData.AddRoundedCube(Vector3.up * 5, new Vector3(4, 2, 4), 4);
        meshData.subMeshes.Add(meshData.triangles.Count - 1);
        meshData.AddCube(Vector3.zero, new Vector3(1, 5, 1));
        meshData.subMeshes.Add(meshData.triangles.Count - 1);
        
        meshData.ScaleMesh(Vector3.one * 0.5f);

        return meshData;
    }
}