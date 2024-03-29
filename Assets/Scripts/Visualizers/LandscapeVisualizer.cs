﻿using System.Collections.Generic;
using Freya;
using MeshGen;
using UnityEngine;

public class LandscapeVisualizer : DungeonVisualizer
{
    [SerializeField] private NoiseMapData noiseMapData;
    [SerializeField] private Material groundMaterial;
    [SerializeField] private Material rockMaterial;
    [SerializeField] private Material bushMaterial;
    [SerializeField] private Material trunkMaterial;

    private NoiseMap noiseMap;
    private List<Vector3> positionSamples;
    
    protected override void Visualize(Dungeon dungeon)
    {
        noiseMap = new NoiseMap(noiseMapData);
        positionSamples = PoissonDiscSampling.GeneratePoints(3, dungeon.bound);
        
        new TerrainGenerator(dungeon, noiseMap, groundMaterial).Generate(); //generate terrain
        new RoomMeshGenerator(dungeon, noiseMap, trunkMaterial).Generate();
        new TreeMesh(new [] { bushMaterial, trunkMaterial }, noiseMap, positionSamples).PlaceMeshes(dungeon); // generate bushes
        new RockMesh(new [] { rockMaterial }, noiseMap, positionSamples).PlaceMeshes(dungeon); // generate rocks

        var stairLine = new LineSegment3D(new Vector3(-13, 0, 0), new Vector3(-13, 10, 20));
        var mesh = StairMeshGenerator.GenerateStair(stairLine, 10, 5f);
        MeshUtils.InstantiateMeshGameObject("stairs", mesh, groundMaterial);
    }
}