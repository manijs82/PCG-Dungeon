using System.Collections.Generic;
using UnityEngine;

public class LandscapeVisualizer : DungeonVisualizer
{
    [SerializeField] private HeightMapData heightMapData;
    [SerializeField] private Material groundMaterial;
    [SerializeField] private Material rockMaterial;
    [SerializeField] private Material bushMaterial;
    [SerializeField] private Material trunkMaterial;

    private HeightMap heightMap;
    private List<Vector3> positionSamples;
    
    protected override void Visualize(Dungeon dungeon)
    {
        heightMap = new HeightMap(heightMapData);
        positionSamples = PoissonDiscSampling.GeneratePoints(3, dungeon.bound, 300000);
        
        TerrainGenerator terrainGenerator = new TerrainGenerator(dungeon, heightMap); //generate terrain
        var meshes = terrainGenerator.GenerateTerrainSections();
        
        foreach (var mesh in meshes)
        {
            var go = new GameObject("terrain section");
            go.transform.SetParent(transform);
            var meshFilter = go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>().material = groundMaterial;
            meshFilter.mesh = mesh;
            var meshCollider = go.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }
        
        new TreeMesh(new [] { bushMaterial, trunkMaterial }, heightMap, positionSamples).PlaceMeshes(dungeon); // generate bushes
        new RockMesh(new [] { rockMaterial }, heightMap, positionSamples).PlaceMeshes(dungeon); // generate rocks
    }

    
}