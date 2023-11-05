using UnityEngine;

public class TerrainVisualizer : DungeonVisualizer
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private GrassShell grassShell;

    protected override void Visualize(Dungeon dungeon)
    {
        TerrainGenerator terrainGenerator = new TerrainGenerator(dungeon);
        var mesh = terrainGenerator.GenerateMesh();
        
        meshFilter.mesh = mesh;
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        
        grassShell.Enable(new BackgroundMask(dungeon).GetMaskTexture(1f));
    }
}