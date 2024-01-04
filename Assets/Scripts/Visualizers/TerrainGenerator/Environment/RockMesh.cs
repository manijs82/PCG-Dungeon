using System.Collections.Generic;
using System.Linq;
using MeshGen;
using UnityEngine;
using Utils;

public class RockMesh : EnvironmentMesh
{
    private Dungeon dungeon;
    
    public RockMesh(Material[] materials, NoiseMap noiseMap, List<Vector3> positionSamples) : base(materials, noiseMap, positionSamples)
    {
    }

    public override void PlaceMeshes(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        var meshVariation = GetMeshVariations();
        var positions = GetPositions().ToArray();

        int meshCount = Mathf.CeilToInt(positions.Length / 1000f);
        int lastMeshRockCount = positions.Length % 1000;
        for (int i = 0, p = 0; i < meshCount; i++)
        {
            var rockCount = i == meshCount - 1 ? lastMeshRockCount : 1000;
            var combine = new CombineInstance[rockCount];
            var combinedMesh = new Mesh();
            
            for (var j = 0; j < rockCount; j++, p++)
            {
                if(p >= positions.Length)
                    break;
                
                var position = positions[p];
                var meshPosition = new Vector3(position.x, GetHeightAt((int)position.x, (int)position.y), position.y);

                var mesh = meshVariation[Random.Range(0, meshVariation.Length)];
                combine[j].mesh = mesh;
                combine[j].transform = Matrix4x4.TRS(meshPosition, Quaternion.identity, Vector3.one);
            }
            
            combinedMesh.CombineMeshes(combine, true);
            var go = new GameObject("Rock");
            go.AddComponent<MeshFilter>().mesh = combinedMesh;
            go.AddComponent<MeshRenderer>().materials = materials;
        }
    }

    protected override IEnumerable<Vector3> GetPositions()
    {
        PerlinMask perlinMask = new PerlinMask(dungeon, 0.6f, 5);
        BackgroundMask backgroundMask = new BackgroundMask(dungeon);
        var mask = Mask.GetCombinedMask(CombineMode.Intersection, perlinMask, backgroundMask);

        var maskedPositions = PoissonDiscSampling.GeneratePoints(positionSamples, 2, dungeon.bound, 10000).MaskPositions(mask);
        var valuesToRemove = maskedPositions as Vector3[] ?? maskedPositions.ToArray();
        positionSamples.RemoveValues(valuesToRemove);
        return valuesToRemove;
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