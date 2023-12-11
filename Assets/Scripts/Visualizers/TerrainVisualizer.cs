using UnityEngine;

public class TerrainVisualizer : DungeonVisualizer
{
    [SerializeField] private HeightMapData heightMapData;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private GrassShell grassShell;
    [SerializeField] private Material rockMaterial;
    [SerializeField] private Material bushMaterial;
    [SerializeField] private Material trunkMaterial;

    private HeightMap heightMap;
    
    protected override void Visualize(Dungeon dungeon)
    {
        heightMap = new HeightMap(heightMapData);
        
        TerrainGenerator terrainGenerator = new TerrainGenerator(dungeon, heightMap); //generate terrain
        var meshes = terrainGenerator.GenerateMesh();
        
        foreach (var mesh in meshes)
        {
            meshFilter.mesh = mesh;
            var meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }
        
        grassShell.Enable(new BackgroundMask(dungeon).GetMaskTexture(1f)); // generate grass
        
        new RockMesh(new [] { rockMaterial }).PlaceMeshes(dungeon); // generate rocks
        new TreeMesh(new [] { bushMaterial, trunkMaterial }).PlaceMeshes(dungeon); // generate bushes
    }

    
}