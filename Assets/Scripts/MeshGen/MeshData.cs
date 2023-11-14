﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MeshGen
{
    public class MeshData
    {
        public List<Triangle> triangles = new();
        public List<Vertex> vertices = new();

        private int currentTriangleIndex = 0;
        
        public int AddTriangle(int v1, int v2, int v3)
        {
            if (v1 < 0 || v1 >= vertices.Count ||
                v2 < 0 || v2 >= vertices.Count ||
                v3 < 0 || v3 >= vertices.Count)
            {
                throw new ArgumentException("Invalid vertex index.");
            }
            
            int triangleIndex = currentTriangleIndex;
            Triangle newTriangle = new Triangle(v1, v2, v3, triangleIndex);
            triangles.Add(newTriangle);
            
            SetVertexTriangleIndex(v1, triangleIndex);
            SetVertexTriangleIndex(v2, triangleIndex);
            SetVertexTriangleIndex(v3, triangleIndex);

            for (var i = 0; i < triangles.Count; i++)
            {
                var triangle = triangles[i];
                if (triangle.index == newTriangle.index)
                    continue;

                if (triangle.IsAdjacentTo(newTriangle, out int sharedV1, out int sharedV2))
                {
                    SetTriangleEdge(triangle.index, sharedV1, sharedV2, newTriangle.index);
                    SetTriangleEdge(newTriangle.index, sharedV1, sharedV2, triangle.index);
                }
            }

            currentTriangleIndex++;
            return triangleIndex;
        }

        public void RemoveTriangle(int triangleIndex) //TODO: update vertex tIndex
        {
            var triangle = GetTriangle(triangleIndex);
            triangles.Remove(triangle);

            var ad1 = triangle.adjacentTriangle1;
            var ad2 = triangle.adjacentTriangle2;
            var ad3 = triangle.adjacentTriangle3;

            if (ad1 >= 0) 
                SetTriangleEdge(ad1, triangle.vertex1, triangle.vertex2, -1);
            if (ad2 >= 0) 
                SetTriangleEdge(ad2, triangle.vertex2, triangle.vertex3, -1);
            if (ad3 >= 0) 
                SetTriangleEdge(ad3, triangle.vertex3, triangle.vertex1, -1);
        }

        public Triangle GetTriangle(int triangleIndex)
        {
            return triangles.FirstOrDefault(t => t.index == triangleIndex);
        }
        
        public void AddVertex(float x, float y, float z)
        {
            vertices.Add(new Vertex(new Vector3(x, y, z)));
        }
        
        public int AddVertex(Vector3 pos)
        {
            int vIndex = vertices.Count;
            vertices.Add(new Vertex(pos));

            return vIndex;
        }

        public void SetVertexPosition(int vIndex, Vector3 newPos)
        {
            var vert = vertices[vIndex];

            vert.position = newPos;

            vertices[vIndex] = vert;
        }

        private void SetVertexTriangleIndex(int vIndex, int triangleIndex)
        {
            var vert = vertices[vIndex];
            //if (vert.triangle >= 0)
            //    return;
            
            vert.triangle = triangleIndex;

            vertices[vIndex] = vert;
        }

        private void SetTriangleEdge(int triangle, int edge, int edgeTriangle)
        {
            var tri = GetTriangle(triangle);
            var tIndex = triangles.IndexOf(tri);
            tri.SetAdjacentTriangle(edge, edgeTriangle);

            triangles[tIndex] = tri;
        }
        
        public void SetTriangleEdge(int triangle, int vIndex1, int vIndex2, int edgeTriangle)
        {
            var tri = GetTriangle(triangle);
            var tIndex = triangles.IndexOf(tri);
            tri.SetAdjacentTriangle(vIndex1, vIndex2, edgeTriangle);

            triangles[tIndex] = tri;
        }
        
        public Vector3 GetTriangleCenter(Triangle triangle)
        {
            var v1 = vertices[triangle.vertex1].position;
            var v2 = vertices[triangle.vertex2].position;
            var v3 = vertices[triangle.vertex3].position;

            return (v1 + v2 + v3) / 3f;
        }
        
        public Vector3 GetTriangleNormal(Triangle triangle)
        {
            var v1 = vertices[triangle.vertex1].position;
            var v2 = vertices[triangle.vertex2].position;
            var v3 = vertices[triangle.vertex3].position;

            return Vector3.Cross(v2 - v1, v3 - v1).normalized;
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