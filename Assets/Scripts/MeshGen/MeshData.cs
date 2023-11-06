using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MeshGen
{
    public class MeshData
    {
        public List<Triangle> triangles = new();
        public List<Vertex> vertices = new();

        public int AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int triangleIndex = triangles.Count;
            int vertexIndex = vertices.Count;

            AddVertex(v1);
            AddVertex(v2);
            AddVertex(v3);

            triangles.Add(new Triangle(vertexIndex, vertexIndex + 1, vertexIndex + 2,
                -1, -1, -1));

            return triangleIndex;
        }
        
        /// <param name="v1"> new vert pos </param>
        /// <param name="v2"> existing vert index </param>
        /// <param name="v3"> existing vert index </param>
        /// <returns> created triangle index</returns>
        public int AddTriangle(Vector3 v1, int v2, int v3)
        {
            int triangleIndex = triangles.Count;
            int vertexIndex = vertices.Count;

            AddVertex(v1);

            triangles.Add(new Triangle(v2, vertexIndex, v3,
                -1, -1, -1));
            
            return triangleIndex;
        }
        
        public int AddTriangle(int v1, int v2, int v3)
        {
            int triangleIndex = triangles.Count;

            if (vertices[v1].triangle == -1 && vertices[v2].triangle == -1 && vertices[v3].triangle == -1)
            {
                SetVertexTriangleIndex(v1, triangleIndex);
                SetVertexTriangleIndex(v2, triangleIndex);
                SetVertexTriangleIndex(v3, triangleIndex);
                
                triangles.Add(new Triangle(v1, v2, v3, -1, -1, -1));
                
                return triangleIndex;
            }

            int tri1 = GetTriangle(v1, v3);
            if (tri1 >= 0)
            {
                SetTriangleEdge(tri1, v1, v3, triangleIndex);
                
                triangles.Add(new Triangle(v1, v2, v3,
                    -1, -1, triangleIndex));
                
                SetVertexTriangleIndex(v2, triangleIndex);

                return triangleIndex;
            }
            
            return -1;
        }

        public int AddConnectedTriangle(Vector3 newVert, int triangleIndex, int edgeIndex)
        {
            int newTriangleIndex = triangles.Count;
            int[] otherVerts = triangles[triangleIndex].GetEdgeVertices(edgeIndex);
            
            AddTriangle(newVert, otherVerts[0], otherVerts[1]);
            SetTriangleEdge(newTriangleIndex, 2, triangleIndex);
            SetTriangleEdge(triangleIndex, edgeIndex, newTriangleIndex);
            
            return newTriangleIndex;
        }

        public void AddVertex(Vector3 vertex)
        {
            vertices.Add(new Vertex(vertex, -1));
        }
        
        public void AddVertex(float x, float y, float z)
        {
            vertices.Add(new Vertex(new Vector3(x, y, z), -1));
        }

        public int GetTriangle(int vIndex1, int vIndex2)
        {
            int tri1 = vertices.ToList()[vIndex1].triangle;
            int tri2 = vertices.ToList()[vIndex2].triangle;

            if (tri1 == tri2)
                return tri1;
            if (triangles[tri1].Contains(tri2))
                return tri1;
            if (triangles[tri2].Contains(tri1))
                return tri2;

            return -1;
        }

        public void SetVertexTriangleIndex(int vIndex, int triangleIndex)
        {
            var vert = vertices[vIndex];
            vert.triangle = triangleIndex;

            vertices[vIndex] = vert;
        }

        public void SetTriangleEdge(int triangle, int edge, int edgeTriangle)
        {
            var tri = triangles[triangle];
            tri.SetAdjacentTriangle(edge, edgeTriangle);

            triangles[triangle] = tri;
        }
        
        public void SetTriangleEdge(int triangle, int vIndex1, int vIndex2, int edgeTriangle)
        {
            var tri = triangles[triangle];
            tri.SetAdjacentTriangle(vIndex1, vIndex2, edgeTriangle);

            triangles[triangle] = tri;
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