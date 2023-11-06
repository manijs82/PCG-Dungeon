using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MeshGen
{
    public class MeshGizmo : MonoBehaviour
    {
        private MeshData meshData;
        
        public void SetMeshData(MeshData meshData)
        {
            this.meshData = meshData;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (meshData == null) return;
            
            Handles.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            
            foreach (var triangle in meshData.triangles)
            {
                var pos1 = meshData.vertices[triangle.vertex1].position;
                var pos2 = meshData.vertices[triangle.vertex2].position;
                var pos3 = meshData.vertices[triangle.vertex3].position;

                pos1 += (meshData.vertices[triangle.vertex2].position - meshData.vertices[triangle.vertex1].position) * 0.01f;
                pos1 += (meshData.vertices[triangle.vertex3].position - meshData.vertices[triangle.vertex1].position) * 0.01f;
            
                pos2 += (meshData.vertices[triangle.vertex1].position - meshData.vertices[triangle.vertex2].position) * 0.01f;
                pos2 += (meshData.vertices[triangle.vertex3].position - meshData.vertices[triangle.vertex2].position) * 0.01f;
            
                pos3 += (meshData.vertices[triangle.vertex1].position - meshData.vertices[triangle.vertex3].position) * 0.01f;
                pos3 += (meshData.vertices[triangle.vertex2].position - meshData.vertices[triangle.vertex3].position) * 0.01f;

                Handles.DrawAAPolyLine(3, pos1, pos2, pos3, pos1);

                if (triangle.adjacentTriangle1 >= 0) 
                    DrawLineBetweenTriangles(triangle, triangle.adjacentTriangle1);
                if (triangle.adjacentTriangle2 >= 0) 
                    DrawLineBetweenTriangles(triangle, triangle.adjacentTriangle2);
                if (triangle.adjacentTriangle3 >= 0) 
                    DrawLineBetweenTriangles(triangle, triangle.adjacentTriangle3);
                Handles.color = Color.white;
            }

            foreach (var vertex in meshData.vertices)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(vertex.position, 0.05f);
                
                Handles.color = Color.cyan;
                Handles.DrawAAPolyLine(3, vertex.position, 
                    Vector3.Lerp(vertex.position, GetTriangleCenter(meshData.triangles[vertex.triangle]), 0.2f));
            }
            
            Handles.matrix = Matrix4x4.identity;
        }

        private void DrawLineBetweenTriangles(Triangle triangle1, int triangle2Index)
        {
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(GetTriangleCenter(triangle1),
                GetTriangleCenter(meshData.triangles[triangle2Index]));
        }

        public Vector3 GetTriangleCenter(Triangle triangle)
        {
            var v1 = meshData.vertices[triangle.vertex1].position;
            var v2 = meshData.vertices[triangle.vertex2].position;
            var v3 = meshData.vertices[triangle.vertex3].position;

            return (v1 + v2 + v3) / 3f;
        }
#endif
    }
}