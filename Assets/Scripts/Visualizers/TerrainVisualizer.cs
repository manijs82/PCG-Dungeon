using UnityEngine;

public class TerrainVisualizer : DungeonVisualizer
{
    [SerializeField] private HeightMapData heightMapData;
    [SerializeField] private GrassShell grassShell;
    [SerializeField] private Material rockMaterial;
    [SerializeField] private Material bushMaterial;
    [SerializeField] private Material trunkMaterial;

    private HeightMap heightMap;
    
    protected override void Visualize(Dungeon dungeon)
    {
        heightMap = new HeightMap(heightMapData);
        
        TerrainGenerator terrainGenerator = new TerrainGenerator(dungeon, heightMap); //generate terrain
        var meshes = terrainGenerator.GenerateTerrainSections();
        
        foreach (var mesh in meshes)
        {
            var go = new GameObject("terrain section");
            go.transform.SetParent(transform);
            var meshFilter = go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>().material = rockMaterial;
            meshFilter.mesh = mesh;
            var meshCollider = go.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }
        
        new RockMesh(new [] { rockMaterial }, heightMap).PlaceMeshes(dungeon); // generate rocks
        new TreeMesh(new [] { bushMaterial, trunkMaterial }, heightMap).PlaceMeshes(dungeon); // generate bushes
    }

    
}