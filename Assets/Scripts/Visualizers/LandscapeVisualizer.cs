﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LandscapeVisualizer : DungeonVisualizer
{
    [FormerlySerializedAs("heightMapData")] [SerializeField] private NoiseMapData noiseMapData;
    [SerializeField] private Material groundMaterial;
    [SerializeField] private Material rockMaterial;
    [SerializeField] private Material bushMaterial;
    [SerializeField] private Material trunkMaterial;

    private NoiseMap noiseMap;
    private List<Vector3> positionSamples;
    
    protected override void Visualize(Dungeon dungeon)
    {
        noiseMap = new NoiseMap(noiseMapData);
        positionSamples = PoissonDiscSampling.GeneratePoints(3, dungeon.bound, 300000);
        
        TerrainGenerator terrainGenerator = new TerrainGenerator(dungeon, noiseMap); //generate terrain
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
        
        new TreeMesh(new [] { bushMaterial, trunkMaterial }, noiseMap, positionSamples).PlaceMeshes(dungeon); // generate bushes
        new RockMesh(new [] { rockMaterial }, noiseMap, positionSamples).PlaceMeshes(dungeon); // generate rocks
    }

    
}