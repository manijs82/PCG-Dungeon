using UnityEngine;

namespace MeshGen
{
    public static class MeshUtils
    {
        public static void MoveTriangle(this MeshData meshData, int triangleIndex, Vector3 offset)
        {
            var moveMatrix = Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one);
            meshData.TranslateTriangle(triangleIndex, moveMatrix);
        }

        public static void RotateTriangle(this MeshData meshData, int triangleIndex, Vector3 axis, float angle)
        {
            var newRotation = Quaternion.AngleAxis(angle, axis);
            var rotationMatrix = Matrix4x4.TRS(Vector3.zero, newRotation, Vector3.one);
            meshData.TranslateTriangle(triangleIndex, rotationMatrix);
        }
        
        public static void ScaleTriangle(this MeshData meshData, int triangleIndex, Vector3 scale)
        {
            var scalingMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
            meshData.TranslateTriangle(triangleIndex, scalingMatrix);
        }
        
        public static void TranslateTriangle(this MeshData meshData, int triangleIndex, Matrix4x4 translationMatrix)
        {
            var triangle = meshData.GetTriangle(triangleIndex);
            meshData.GetTriangleVerticesPositions(triangleIndex, out var v1, out var v2, out var v3);
            
            var center = meshData.GetTriangleCenter(triangle);
            var normal = meshData.GetTriangleNormal(triangle);
            var tangent = (v1 - center).normalized;

            var rotation = Quaternion.LookRotation(normal, tangent);
            
            var matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
            var newMatrix = matrix * translationMatrix;
            
            var v1Local = matrix.inverse.MultiplyPoint(v1);
            var v2Local = matrix.inverse.MultiplyPoint(v2);
            var v3Local = matrix.inverse.MultiplyPoint(v3);
            
            meshData.SetVertexPosition(triangle.vertex1, newMatrix.MultiplyPoint(v1Local));
            meshData.SetVertexPosition(triangle.vertex2, newMatrix.MultiplyPoint(v2Local));
            meshData.SetVertexPosition(triangle.vertex3, newMatrix.MultiplyPoint(v3Local));
        }
        
        public static void InsertVertexTriangle(this MeshData meshData, int tIndex)
        {
            var triangle = meshData.GetTriangle(tIndex);
            
            var center = meshData.GetTriangleCenter(triangle);
            var vIndex = meshData.AddVertex(center);
            
            meshData.RemoveTriangle(tIndex);

            meshData.AddTriangle(triangle.vertex1, vIndex, triangle.vertex3);
            meshData.AddTriangle(triangle.vertex2, vIndex, triangle.vertex1);
            meshData.AddTriangle(triangle.vertex3, vIndex, triangle.vertex2);
        }
        
        public static void InsertVertexQuad(this MeshData meshData, int t1Index, int t2Index)
        {
            var t1 = meshData.GetTriangle(t1Index);
            var t2 = meshData.GetTriangle(t2Index);
            if(!t1.IsAdjacentTo(t2, out var edgeV1, out var edgeV2))
                return;
            
            meshData.GetTriangleVerticesPositions(t1Index, out var t1v1, out var t1v2, out var t1v3);
            meshData.GetTriangleVerticesPositions(t2Index, out var t2v1, out var t2v2, out var t2v3);
            
            var center = GetPolygonCenter(t1v1, t1v2, t1v3, t2v1, t2v2, t2v3);
            var vIndex = meshData.AddVertex(center);
            
            meshData.RemoveTriangle(t1Index);
            meshData.RemoveTriangle(t2Index);

            var other = t2.GetOtherVertex(edgeV1, edgeV2);
            switch (t1.GetEdgeIndex(edgeV1, edgeV2))
            {
                case 0:
                    meshData.AddTriangle(t1.vertex1, vIndex, t1.vertex3);
                    meshData.AddTriangle(vIndex, t1.vertex2, t1.vertex3);

                    meshData.AddTriangle(t1.vertex1, other, vIndex);
                    meshData.AddTriangle(other, t1.vertex2, vIndex);
                    break;
                case 1:
                    meshData.AddTriangle(t1.vertex2, vIndex, t1.vertex1);
                    meshData.AddTriangle(vIndex, t1.vertex3, t1.vertex1);

                    meshData.AddTriangle(t1.vertex3, vIndex, other);
                    meshData.AddTriangle(vIndex, t1.vertex2, other);
                    break;
                case 2:
                    meshData.AddTriangle(t1.vertex1, t1.vertex2, vIndex);
                    meshData.AddTriangle(t1.vertex2, t1.vertex3, vIndex);

                    meshData.AddTriangle(t1.vertex3, other, vIndex);
                    meshData.AddTriangle(other, t1.vertex1, vIndex);
                    break;
            }
        }
        
        public static Vector3 GetTriangleCenter(this MeshData meshData, int triangle)
        {
            meshData.GetTriangleVerticesPositions(triangle, out var v1, out var v2, out var v3);
            return (v1 + v2 + v3) / 3f;
        }
        
        public static Vector3 GetTriangleNormal(this MeshData meshData, int triangle)
        {
            meshData.GetTriangleVerticesPositions(triangle, out var v1, out var v2, out var v3);
            return Vector3.Cross(v2 - v1, v3 - v1).normalized;
        }
        
        public static Vector3 GetPolygonCenter(params Vector3[] points)
        {
            return GetPositionSum(points) / points.Length;
        }
        
        public static Vector3 GetPositionSum(Vector3[] points)
        {
            Vector3 sum = Vector3.zero;
            foreach (var point in points)
                sum += point;
            return sum;
        }
        
        public static void GetTriangleVerticesPositions(this MeshData meshData, int triangle, out Vector3 v1, out Vector3 v2, out Vector3 v3)
        {
            v1 = meshData.vertices[meshData.GetTriangle(triangle).vertex1].position;
            v2 = meshData.vertices[meshData.GetTriangle(triangle).vertex2].position;
            v3 = meshData.vertices[meshData.GetTriangle(triangle).vertex3].position;
        }
        
        public static void GetTriangleVertices(this MeshData meshData, int triangle, out Vertex v1, out Vertex v2, out Vertex v3)
        {
            v1 = meshData.vertices[meshData.GetTriangle(triangle).vertex1];
            v2 = meshData.vertices[meshData.GetTriangle(triangle).vertex2];
            v3 = meshData.vertices[meshData.GetTriangle(triangle).vertex3];
        }
    }
}