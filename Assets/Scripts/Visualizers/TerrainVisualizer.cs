using UnityEngine;

public class TerrainVisualizer : DungeonVisualizer
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private GrassShell grassShell;
    [SerializeField] private Material rockMaterial;
    
    protected override void Visualize(Dungeon dungeon)
    {
        TerrainGenerator terrainGenerator = new TerrainGenerator(dungeon); //generate terrain
        var mesh = terrainGenerator.GenerateMesh();
        
        meshFilter.mesh = mesh;
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        
        grassShell.Enable(new BackgroundMask(dungeon).GetMaskTexture(1f)); // generate grass
        new RockMesh(rockMaterial).PlaceMeshes(dungeon); // generate rocks
    }
}