using System.Collections.Generic;
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
        positionSamples = PoissonDiscSampling.GeneratePoints(3, dungeon.bound, 300000);
        
        new TerrainGenerator(dungeon, noiseMap, groundMaterial).Generate(); //generate terrain
        new RoomMeshGenerator(dungeon, noiseMap, trunkMaterial).Generate();
        new TreeMesh(new [] { bushMaterial, trunkMaterial }, noiseMap, positionSamples).PlaceMeshes(dungeon); // generate bushes
        new RockMesh(new [] { rockMaterial }, noiseMap, positionSamples).PlaceMeshes(dungeon); // generate rocks
    }
}