using System.Collections.Generic;
using UnityEngine;

namespace MeshGen
{
    public class MeshData
    {
        public List<Triangle> triangles = new();
        public HashSet<Vertex> vertices = new();

        public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int triangleIndex = triangles.Count;
            int vertexIndex = vertices.Count;

            AddVertex(v1, triangleIndex);
            AddVertex(v2, triangleIndex);
            AddVertex(v3, triangleIndex);

            triangles.Add(new Triangle(vertexIndex, vertexIndex + 1, vertexIndex + 2,
                -1, -1, -1));
        }
        
        public void AddTriangle(Vector3 v1, int v2, int v3)
        {
            int triangleIndex = triangles.Count;
            int vertexIndex = vertices.Count;

            AddVertex(v1, triangleIndex);

            triangles.Add(new Triangle(v2, vertexIndex, v3,
                -1, -1, -1));
        }

        public void AddConnectedTriangle(Vector3 newVert, int triangleIndex, int edgeIndex)
        {
            int newTriangleIndex = triangles.Count;
            int newVertexIndex = vertices.Count;
            int[] otherVerts = triangles[triangleIndex].GetEdgeVertices(edgeIndex);
            
            AddTriangle(newVert, otherVerts[0], otherVerts[1]);
            triangles[newTriangleIndex].SetAdjacentTriangle(2, triangleIndex);
            triangles[triangleIndex].SetAdjacentTriangle(newTriangleIndex, edgeIndex);
        }

        public void AddVertex(Vector3 vertex, int triangleIndex)
        {
            vertices.Add(new Vertex(vertex, triangleIndex));
        }

        public Mesh CreateMesh()
        {
            var verts = new List<Vector3>();
            foreach (var vertex in vertices) verts.Add(vertex.position);

            var tris = new List<int>();
            foreach (var triangle in triangles)
            {
                tris.Add(triangle.vertex1);
                tris.Add(triangle.vertex2);
                tris.Add(triangle.vertex3);
            }

            Mesh mesh = new Mesh
            {
                vertices = verts.ToArray(),
                triangles = tris.ToArray(),
            };

            mesh.RecalculateNormals();
            return mesh;
        }
    }
}