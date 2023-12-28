using Freya;
using MeshGen;
using UnityEngine;

public static class StairMeshGenerator
{
    public static Mesh GenerateStair(LineSegment3D line, int stairCount, float width)
    {
        MeshData meshData = new MeshData();

        var firstStairLine = (line.GetPoint(1 / (stairCount - 1f)) - line.start);
        float stairHeight = firstStairLine.y;
        float stairDepth = new Vector3(firstStairLine.x, line.start.y, firstStairLine.z).magnitude;
        Vector3 lineForward = (line.end - new Vector3(line.start.x, line.end.y, line.start.z)).normalized;
        Vector3 lineRight = Vector3.Cross(Vector3.up, lineForward) * (width / 2f);
        for (int i = 0; i < stairCount; i++)
        {
            var t = i / (stairCount - 1f);
            
            var center = line.GetPoint(t);
            
            meshData.AddQuad(center - lineForward * stairDepth / 2f, lineRight, Vector3.up * stairHeight / 2f);
            meshData.AddQuad(center + Vector3.up * stairHeight / 2f, lineRight, lineForward * stairDepth / 2f);
        }

        var mesh = meshData.CreateMesh(false);
        return mesh;
    }
}