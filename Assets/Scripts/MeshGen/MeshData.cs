using System.Collections.Generic;
using UnityEngine;

namespace MeshGen
{
    public class MeshData
    {
        public List<Vector3> vertices = new();
        public List<int> triangles = new();
        public List<Vector2> uvs = new();

        public void AddTriangle(int a, int b, int c)
        {
            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);
        }
    
        public void AddVertex(Vector3 vertex)
        {
            vertices.Add(vertex);
        }
    
        public void AddUV(Vector2 uv)
        {
            uvs.Add(uv);
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray(),
                uv = uvs.ToArray()
            };

            mesh.RecalculateNormals();
            return mesh;
        }
    }

    public struct Triangle
    {
        public Vertex vertex1;
        public Vertex vertex2;
        public Vertex vertex3;

        public Edge edge1;
        public Edge edge2;
        public Edge edge3;
    }

    public struct Vertex
    {
    
    }

    public struct Edge
    {
    
    }
}