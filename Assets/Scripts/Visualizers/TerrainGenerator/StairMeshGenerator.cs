using Freya;
using MeshGen;
using UnityEngine;
using Utils;

public static class StairMeshGenerator
{
    public static Mesh GenerateStair(LineSegment3D stairCaseLine, int stairCount, float width)
    {
        MeshData meshData = new MeshData();
        StairCase stairCase = new StairCase(stairCaseLine, stairCount, width);

        foreach (var stair in stairCase.stairs)
        {
            meshData.AddQuad(stair.center - stair.forward, stair.right, stair.up); // front face
            meshData.AddQuad(stair.center + stair.up, stair.right, stair.forward); // upper face

            float heightClimbed = stairCaseLine.GetPoint(stair.index / (stairCount - 1f)).y - stairCaseLine.start.y;
            meshData.AddQuad(stair.center.OffsetDown(heightClimbed / 2f) + stair.right, stair.forward, Vector3.up * (heightClimbed / 2f + stairCase.stairHeight / 2f)); // right face
            meshData.AddQuad(stair.center.OffsetDown(heightClimbed / 2f) - stair.right, -stair.forward, Vector3.up * (heightClimbed / 2f + stairCase.stairHeight / 2f)); // left face
        }

        var mesh = meshData.CreateMesh(false, true);
        return mesh;
    }

    private struct StairCase
    {
        public LineSegment3D stairCaseLine;
        public int stairCount;
        public float width;
        
        public readonly Stair[] stairs;
        public readonly float stairHeight;
        public readonly float stairDepth;

        public StairCase(LineSegment3D stairCaseLine, int stairCount, float width)
        {
            this.stairCaseLine = stairCaseLine;
            this.stairCount = stairCount;
            this.width = width;

            stairs = new Stair[stairCount];
            
            var firstStairLine = (stairCaseLine.GetPoint(1 / (stairCount - 1f)) - stairCaseLine.start);
            stairHeight = firstStairLine.y;
            stairDepth = new Vector3(firstStairLine.x, stairCaseLine.start.y, firstStairLine.z).magnitude;
            Vector3 lineForward = (stairCaseLine.end - new Vector3(stairCaseLine.start.x, stairCaseLine.end.y, stairCaseLine.start.z)).normalized;
            Vector3 lineRight = Vector3.Cross(Vector3.up, lineForward) * (width / 2f);
            
            for (int i = 0; i < stairCount; i++)
            {
                var t = i / (stairCount - 1f);
                var center = stairCaseLine.GetPoint(t);
                var halfForward = lineForward * stairDepth / 2f;

                stairs[i] = new Stair(i, center, halfForward, lineRight, stairHeight);
            }
        }
    }

    private struct Stair
    {
        public int index;
        public readonly Vector3 center;
        public readonly Vector3 forward;
        public readonly Vector3 right;
        public readonly Vector3 up;

        public Stair(int index, Vector3 center, Vector3 forward, Vector3 right, float height)
        {
            this.index = index;
            this.center = center;
            this.forward = forward;
            this.right = right;
            this.up = Vector3.up * height / 2f;
        }
    }
}