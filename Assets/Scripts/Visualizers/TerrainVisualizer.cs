using System.Linq;
using MeshGen;
using UnityEditor;
using UnityEngine;

public class TerrainVisualizer : DungeonVisualizer
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private GrassShell grassShell;
    [SerializeField] private Material rockMaterial;

    private MeshData meshData;
    
    protected override void Visualize(Dungeon dungeon)
    {
        TerrainGenerator terrainGenerator = new TerrainGenerator(dungeon); //generate terrain
        var mesh = terrainGenerator.GenerateMesh();
        
        meshFilter.mesh = mesh;
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        
        grassShell.Enable(new BackgroundMask(dungeon).GetMaskTexture(1f)); // generate grass
        var rockMesh = new RockMesh(rockMaterial); // generate rocks
        rockMesh.PlaceMeshes(dungeon);
        meshData = rockMesh.mesh;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (meshData == null) return;

        var verts = meshData.vertices.ToList();
        foreach (var triangle in meshData.triangles)
        {
            var pos1 = verts[triangle.vertex1].position;
            var pos2 = verts[triangle.vertex2].position;
            var pos3 = verts[triangle.vertex3].position;

            pos1 += (verts[triangle.vertex2].position - verts[triangle.vertex1].position) * 0.01f;
            pos1 += (verts[triangle.vertex3].position - verts[triangle.vertex1].position) * 0.01f;
            
            pos2 += (verts[triangle.vertex1].position - verts[triangle.vertex2].position) * 0.01f;
            pos2 += (verts[triangle.vertex3].position - verts[triangle.vertex2].position) * 0.01f;
            
            pos3 += (verts[triangle.vertex1].position - verts[triangle.vertex3].position) * 0.01f;
            pos3 += (verts[triangle.vertex2].position - verts[triangle.vertex3].position) * 0.01f;

            Handles.color = triangle.adjacentTriangle1 >= 0 ? Color.blue : Color.white;
            Handles.DrawAAPolyLine(pos1, pos2);
            Handles.color = triangle.adjacentTriangle2 >= 0 ? Color.blue : Color.white;
            Handles.DrawAAPolyLine(pos2, pos3);
            Handles.color = triangle.adjacentTriangle3 >= 0 ? Color.blue : Color.white;
            Handles.DrawAAPolyLine(pos3, pos1);
            Handles.color = Color.white;
        }
    }
    #endif
}