using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshGen
{
    public class MeshDataOld
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
}