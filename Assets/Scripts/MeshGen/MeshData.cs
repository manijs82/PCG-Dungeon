using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MeshGen
{
    public class MeshData
    {
        public List<Triangle> triangles = new();
        public List<Vertex> vertices = new();
        public List<int> subMeshes = new();

        private int currentTriangleIndex = 0;

        public static MeshData Clone(MeshData toCopy)
        {
            MeshData meshData = new MeshData();
            meshData.triangles = new List<Triangle>(meshData.triangles);
            meshData.vertices = new List<Vertex>(meshData.vertices);
            meshData.subMeshes = new List<int>(meshData.subMeshes);
            meshData.currentTriangleIndex = toCopy.currentTriangleIndex;

            return meshData;
        }

        public void Add(MeshData toAdd)
        {
            var vertexCount = vertices.Count;
            var triangleCount = triangles.Count;
            
            foreach (var vertex in toAdd.vertices)
            {
                var v = vertex;
                v.triangle += currentTriangleIndex;
                vertices.Add(v);
            }

            foreach (var subMesh in toAdd.subMeshes)
            {
                subMeshes.Add(subMesh + triangleCount + 1);
            }
            
            foreach (var triangle in toAdd.triangles)
            {
                var t = new Triangle
                (
                    triangle.vertex1 + vertexCount,
                    triangle.vertex2 + vertexCount,
                    triangle.vertex3 + vertexCount,
                    currentTriangleIndex,
                    triangle.adjacentTriangle1 + currentTriangleIndex,
                    triangle.adjacentTriangle2 + currentTriangleIndex,
                    triangle.adjacentTriangle3 + currentTriangleIndex
                );
                
                triangles.Add(t);
                currentTriangleIndex++;
            }
        }
        
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

            for (int i = 0, c = 0; i < triangles.Count; i++)
            {
                if(c >= 3) break;
                var triangle = triangles[i];
                if (triangle.index == newTriangle.index) continue;
                
                if (triangle.IsAdjacentTo(newTriangle, out int sharedV1, out int sharedV2))
                {
                    SetTriangleEdge(triangle.index, sharedV1, sharedV2, newTriangle.index);
                    SetTriangleEdge(newTriangle.index, sharedV1, sharedV2, triangle.index);

                    c++;
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
            {
                SetTriangleEdge(ad1, triangle.vertex1, triangle.vertex2, -1);
                SetVertexTriangleIndex(triangle.vertex1, ad1);
                SetVertexTriangleIndex(triangle.vertex2, ad1);
            }
            if (ad2 >= 0)
            {
                SetTriangleEdge(ad2, triangle.vertex2, triangle.vertex3, -1);
                SetVertexTriangleIndex(triangle.vertex2, ad2);
                SetVertexTriangleIndex(triangle.vertex3, ad2);
            }
            if (ad3 >= 0)
            {
                SetTriangleEdge(ad3, triangle.vertex3, triangle.vertex1, -1);
                SetVertexTriangleIndex(triangle.vertex3, ad3);
                SetVertexTriangleIndex(triangle.vertex1, ad3);
            }
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
        
        public void SetTriangleEdge(int triangle, int vIndex1, int vIndex2, int edgeTriangle)
        {
            int index = -1;
            for (int i = triangle; i >= 0; i--)
            {
                if(i >= triangles.Count) continue;
                if (triangles[i].index != triangle) continue;

                index = i;
                break;
            }

            if (index == -1)
                return;
            
            var tri = triangles[index];
            tri.SetAdjacentTriangle(vIndex1, vIndex2, edgeTriangle);
            triangles[index] = tri;
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

        public Mesh CreateMesh(bool autoNormals = true, bool perTriangleVertices = false)
        {
            var verts = new List<Vector3>();
            var tris = new List<List<int>>();
            var normals = new List<Vector3>();
            if(subMeshes.Count == 0) subMeshes.Add(triangles.Count - 1);
            
            if(!perTriangleVertices)
            {
                foreach (var vertex in vertices) 
                    verts.Add(vertex.position);

                for (int i = 0; i < subMeshes.Count; i++)
                {
                    var subMeshTris = new List<int>();
                    var lastIndex = i == 0 ? 0 : subMeshes[i - 1] + 1;

                    for (int j = lastIndex; j <= subMeshes[i]; j++)
                    {
                        var triangle = triangles[j];
                        subMeshTris.Add(triangle.vertex1);
                        subMeshTris.Add(triangle.vertex2);
                        subMeshTris.Add(triangle.vertex3);
                    }
                    
                    tris.Add(subMeshTris);
                }
            }
            else
            {
                for (int i = 0; i < subMeshes.Count; i++)
                {
                    var subMeshTris = new List<int>();
                    var lastIndex = i == 0 ? 0 : subMeshes[i - 1] + 1;
                    
                    for (int j = lastIndex; j <= subMeshes[i]; j++)
                    {
                        var triangle = triangles[j];
                        int lastVertIndex = verts.Count;
                    
                        verts.Add(vertices[triangle.vertex1].position);
                        verts.Add(vertices[triangle.vertex2].position);
                        verts.Add(vertices[triangle.vertex3].position);
                    
                        if(!autoNormals)
                        {
                            normals.Add(GetTriangleNormal(triangle));
                            normals.Add(GetTriangleNormal(triangle));
                            normals.Add(GetTriangleNormal(triangle));
                        }
                    
                        subMeshTris.Add(lastVertIndex);
                        subMeshTris.Add(lastVertIndex + 1);
                        subMeshTris.Add(lastVertIndex + 2);
                    }
                    
                    tris.Add(subMeshTris);
                }
            }

            Mesh mesh = new Mesh
            {
                vertices = verts.ToArray()
            };
            mesh.subMeshCount = tris.Count;
            for (var i = 0; i < tris.Count; i++)
            {
                mesh.SetTriangles(tris[i].ToArray(), i);
            }

            if (autoNormals)
                mesh.RecalculateNormals();
            else
                mesh.normals = normals.ToArray();
            return mesh;
        }
    }
}