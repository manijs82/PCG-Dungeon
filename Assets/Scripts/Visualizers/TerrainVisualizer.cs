using UnityEngine;

public class TerrainVisualizer : DungeonVisualizer
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private GrassShell grassShell;
    [SerializeField] private Material rockMaterial;
    [SerializeField] private Material bushMaterial;
    [SerializeField] private Material trunkMaterial;
    
    protected override void Visualize(Dungeon dungeon)
    {
        TerrainGenerator terrainGenerator = new TerrainGenerator(dungeon); //generate terrain
        var mesh = terrainGenerator.GenerateMesh();
        
        meshFilter.mesh = mesh;
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        
        grassShell.Enable(new BackgroundMask(dungeon).GetMaskTexture(1f)); // generate grass
        new RockMesh(new [] { rockMaterial }).PlaceMeshes(dungeon); // generate rocks
        new TreeMesh(new [] { bushMaterial, trunkMaterial }).PlaceMeshes(dungeon); // generate bushes
    }

    
}