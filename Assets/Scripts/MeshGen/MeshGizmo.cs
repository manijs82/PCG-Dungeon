using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace MeshGen
{
    public class MeshGizmo : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos = true;
        
        private MeshData meshData;
        
        public void SetMeshData(MeshData meshData)
        {
            this.meshData = meshData;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (meshData == null || !drawGizmos) return;
            
            Handles.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Handles.zTest = CompareFunction.LessEqual;
            
            foreach (var triangle in meshData.triangles)
            {
                var pos1 = meshData.vertices[triangle.vertex1].position;
                var pos2 = meshData.vertices[triangle.vertex2].position;
                var pos3 = meshData.vertices[triangle.vertex3].position;
                var triangleNormal = meshData.GetTriangleNormal(triangle);

                pos1 += (meshData.vertices[triangle.vertex2].position - meshData.vertices[triangle.vertex1].position) * 0.01f;
                pos1 += (meshData.vertices[triangle.vertex3].position - meshData.vertices[triangle.vertex1].position) * 0.01f;
                pos1 += triangleNormal * 0.01f;
            
                pos2 += (meshData.vertices[triangle.vertex1].position - meshData.vertices[triangle.vertex2].position) * 0.01f;
                pos2 += (meshData.vertices[triangle.vertex3].position - meshData.vertices[triangle.vertex2].position) * 0.01f;
                pos2 += triangleNormal * 0.01f;
            
                pos3 += (meshData.vertices[triangle.vertex1].position - meshData.vertices[triangle.vertex3].position) * 0.01f;
                pos3 += (meshData.vertices[triangle.vertex2].position - meshData.vertices[triangle.vertex3].position) * 0.01f;
                pos3 += triangleNormal * 0.01f;

                Handles.DrawAAPolyLine(3, pos1, pos2, pos3, pos1);

                // draw lines to adjacent triangles of this triangle
                if (triangle.adjacentTriangle1 >= 0) 
                    DrawLineToEdge(triangle, 0);
                if (triangle.adjacentTriangle2 >= 0) 
                    DrawLineToEdge(triangle, 1);
                if (triangle.adjacentTriangle3 >= 0) 
                    DrawLineToEdge(triangle, 2);
                
                Handles.color = Color.cyan;
                
                // draw normal of triangle
                Handles.DrawAAPolyLine(3, GetTriangleCenter(triangle) + triangleNormal * 0.01f, GetTriangleCenter(triangle) + triangleNormal / 4f);
                
                Handles.color = Color.white;
            }

            foreach (var vertex in meshData.vertices)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(vertex.position, 0.03f);
                
                Handles.color = Color.yellow;
                if(vertex.triangle >= 0)
                {
                    var triangle = meshData.GetTriangle(vertex.triangle);
                    var triangleNormal = meshData.GetTriangleNormal(triangle);
                    var position = vertex.position;
                    position += triangleNormal * 0.01f;
                    
                    Handles.DrawAAPolyLine(3, 
                         position,
                                    Vector3.Lerp(position, GetTriangleCenter(triangle) + triangleNormal * 0.01f, 0.4f));
                }
            }
            
            Handles.color = Color.white;
            Gizmos.color = Color.white;
            Handles.matrix = Matrix4x4.identity;
            Handles.zTest = CompareFunction.Always;
        }

        private void DrawLineToEdge(Triangle triangle, int edgeIndex)
        {
            Handles.color = Color.red;
            
            var triangleNormal = meshData.GetTriangleNormal(triangle);
            Handles.DrawAAPolyLine(3, 
                GetTriangleCenter(triangle) + triangleNormal * 0.01f,
                           GetTriangleEdgeCenter(triangle, edgeIndex) + triangleNormal * 0.01f);
        }

        public Vector3 GetTriangleCenter(Triangle triangle)
        {
            var v1 = meshData.vertices[triangle.vertex1].position;
            var v2 = meshData.vertices[triangle.vertex2].position;
            var v3 = meshData.vertices[triangle.vertex3].position;

            return (v1 + v2 + v3) / 3f;
        }
        
        public Vector3 GetTriangleEdgeCenter(Triangle triangle, int edgeIndex)
        {
            var edge = triangle.GetEdgeVertices(edgeIndex);
            
            var v1 = meshData.vertices[edge[0]].position;
            var v2 = meshData.vertices[edge[1]].position;

            return (v1 + v2) / 2f;
        }
#endif
    }
}