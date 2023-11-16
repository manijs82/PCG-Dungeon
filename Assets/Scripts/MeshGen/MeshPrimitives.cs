using UnityEngine;

namespace MeshGen
{
    public static class MeshPrimitives
    {
        #region OldMeshData

        public static void AddCube(this MeshDataOld meshData, Vector3 center, Vector3 halfExtents)
        {
            Vector3 right = new Vector3(halfExtents.x, 0, 0);
            Vector3 up = new Vector3(0, halfExtents.y, 0);
            Vector3 forward = new Vector3(0, 0, halfExtents.z);
            
            meshData.AddQuad(center + up, halfExtents.x, halfExtents.z, Vector3.right, Vector3.forward); // top quad
            meshData.AddQuad(center - up, halfExtents.x, halfExtents.z, Vector3.right, Vector3.back); // bottom quad
            meshData.AddQuad(center +  right, halfExtents.z, halfExtents.y, Vector3.forward, Vector3.up); // right quad
            meshData.AddQuad(center -  right, halfExtents.z, halfExtents.y, Vector3.back, Vector3.up); // left quad
            meshData.AddQuad(center +  forward, halfExtents.x, halfExtents.y, Vector3.left, Vector3.up); // front quad
            meshData.AddQuad(center -  forward, halfExtents.x, halfExtents.y, Vector3.right, Vector3.up); // back quad
        }
        
        public static void AddQuad(this MeshDataOld meshData, Vector3 center, float halfWidth, float halfHeight, Vector3 right, Vector3 up)
        {
            int triangleOffset = meshData.vertices.Count;
            Vector3 halfRight = right * halfWidth;
            Vector3 halfUp = up * halfHeight;
            
            meshData.AddVertex(center - halfRight - halfUp); // bottom left
            meshData.AddVertex(center - halfRight + halfUp); // top left
            meshData.AddVertex(center + halfRight - halfUp); // bottom right
            meshData.AddVertex(center + halfRight + halfUp); // top right
            
            meshData.AddTriangle(triangleOffset, triangleOffset + 1, triangleOffset + 2);
            meshData.AddTriangle(triangleOffset + 1, triangleOffset + 3, triangleOffset + 2);
            
            meshData.AddUV(Vector2.zero);
            meshData.AddUV(Vector2.up);
            meshData.AddUV(Vector2.right);
            meshData.AddUV(Vector2.one);
        }
        
        #endregion

        public static void AddQuad(this MeshData meshData, Vector3 center, Vector3 right, Vector3 up)
        {
            int triangleOffset = meshData.vertices.Count;
            
            meshData.AddVertex(center - right - up); // bottom left
            meshData.AddVertex(center - right + up); // top left
            meshData.AddVertex(center + right - up); // bottom right
            meshData.AddVertex(center + right + up); // top right
            
            meshData.AddTriangle(triangleOffset, triangleOffset + 1, triangleOffset + 2);
            meshData.AddTriangle(triangleOffset + 1, triangleOffset + 3, triangleOffset + 2);
        }

        public static void AddCube(this MeshData meshData, Vector3 center, Vector3 halfExtents)
        {
            int triangleOffset = meshData.vertices.Count;

            int tIndex(int index) => triangleOffset + index; 

            meshData.AddVertex(center + new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z)); // back bottom left 0
            meshData.AddVertex(center + new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z)); // back top left 1
            meshData.AddVertex(center + new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z)); // back bottom right 2
            meshData.AddVertex(center + new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z)); // back top right 3
            
            meshData.AddVertex(center + new Vector3(-halfExtents.x, -halfExtents.y, halfExtents.z)); // front bottom left 4
            meshData.AddVertex(center + new Vector3(-halfExtents.x, halfExtents.y, halfExtents.z)); // front top left 5
            meshData.AddVertex(center + new Vector3(halfExtents.x, -halfExtents.y, halfExtents.z)); // front bottom right 6
            meshData.AddVertex(center + new Vector3(halfExtents.x, halfExtents.y, halfExtents.z)); // front top right 7
            
            // back
            meshData.AddTriangle(tIndex(0), tIndex(1), tIndex(2)); // 0
            meshData.AddTriangle(tIndex(1), tIndex(3), tIndex(2)); // 1
            
            // front
            meshData.AddTriangle(tIndex(6), tIndex(7), tIndex(4)); // 2
            meshData.AddTriangle(tIndex(7), tIndex(5), tIndex(4)); // 3
            
            // left
            meshData.AddTriangle(tIndex(4), tIndex(5), tIndex(0)); // 4
            meshData.AddTriangle(tIndex(5), tIndex(1), tIndex(0)); // 5
            
            // right
            meshData.AddTriangle(tIndex(2), tIndex(3), tIndex(6)); // 6
            meshData.AddTriangle(tIndex(3), tIndex(7), tIndex(6)); // 7
            
            // bottom
            meshData.AddTriangle(tIndex(4), tIndex(0), tIndex(6)); // 8
            meshData.AddTriangle(tIndex(0), tIndex(2), tIndex(6)); // 9
            
            // top
            meshData.AddTriangle(tIndex(1), tIndex(5), tIndex(3)); // 10
            meshData.AddTriangle(tIndex(5), tIndex(7), tIndex(3)); // 11
        }
        
        public static void AddRoundedCube(this MeshData meshData, Vector3 center, Vector3 halfExtents, int roundness)
        {
            meshData.AddSubdividedCube(center, halfExtents, 8, 8, 8, roundness);
        }

        public static void AddSubdividedCube(this MeshData meshData, Vector3 center, Vector3 halfExtents, int xSize, int ySize, int zSize, int roundness = 0)
        {
            int startVIndex = meshData.vertices.Count;
            
            Matrix4x4 matrix = Matrix4x4.TRS(center - halfExtents, Quaternion.identity, new Vector3(halfExtents.x / xSize * 2, halfExtents.y / ySize * 2, halfExtents.z / zSize * 2));
            Vector3 getPoint(float x, float y, float z)
            {
                var point = new Vector3(x, y, z);

                var inner = point;
                if (x < roundness) 
                {
                    inner.x = roundness;
                }
                else if (x > xSize - roundness) 
                {
                    inner.x = xSize - roundness;
                }
                if (y < roundness) 
                {
                    inner.y = roundness;
                }
                else if (y > ySize - roundness) 
                {
                    inner.y = ySize - roundness;
                }
                if (z < roundness) 
                {
                    inner.z = roundness;
                }
                else if (z > zSize - roundness) 
                {
                    inner.z = zSize - roundness;
                }
                
                var normal = (point - inner).normalized;
                return matrix.MultiplyPoint(inner + normal * roundness);
            }

            // add vertices layer by layer from bottom to top
            for (int y = 0; y <= ySize; y++)
            {
                for (int x = 0; x <= xSize; x++) 
                    meshData.AddVertex(getPoint(x, y, 0));
                for (int z = 1; z <= zSize; z++) 
                    meshData.AddVertex(getPoint(xSize, y, z));
                for (int x = xSize - 1; x >= 0; x--) 
                    meshData.AddVertex(getPoint(x, y, zSize));
                for (int z = zSize - 1; z > 0; z--) 
                    meshData.AddVertex(getPoint(0, y, z));
            }
            
            // add top face vertices
            for (int z = 1; z < zSize; z++)
            for (int x = 1; x < xSize; x++)
                meshData.AddVertex(getPoint(x, ySize, z));

            // add bottom face vertices
            for (int z = 1; z < zSize; z++)
            for (int x = 1; x < xSize; x++)
                meshData.AddVertex(getPoint(x, 0, z));
            
            int ring = (xSize + zSize) * 2;
            int v = startVIndex;

            // triangulate each ring
            for (int y = 0; y < ySize; y++, v++)
            {
                for (int q = 0; q < ring - 1; q++, v++) 
                    meshData.SetAsQuad(v, v + ring, v + 1, v + ring + 1);
                meshData.SetAsQuad(v, v + ring, v - ring + 1, v + 1);
            }
            
            // triangulate top and bottom face
            CreateTopFace(meshData, xSize, ySize, zSize, ring, startVIndex);
            CreateBottomFace(meshData, xSize, zSize, ring, startVIndex);
        }

        private static void CreateTopFace(MeshData meshData, int xSize, int ySize, int zSize, int ring, int vIndex)
        {
            int v = vIndex + ring * ySize;
            for (int x = 0; x < xSize - 1; x++, v++)
                meshData.SetAsQuad(v, v + ring - 1, v + 1, v + ring);
            meshData.SetAsQuad(v, v + ring - 1, v + 1, v + 2);

            int vMin = vIndex + ring * (ySize + 1) - 1;
            int vMid = vMin + 1;
            int vMax = v + 2;

            for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
            {
                meshData.SetAsQuad(vMin, vMin - 1, vMid, vMid + xSize - 1);
                for (int x = 1; x < xSize - 1; x++, vMid++)
                    meshData.SetAsQuad(vMid, vMid + xSize - 1, vMid + 1, vMid + xSize);
                meshData.SetAsQuad(vMid, vMid + xSize - 1, vMax, vMax + 1);
            }

            int vTop = vMin - 2;
            meshData.SetAsQuad(vMin, vTop + 1, vMid, vTop);
            for (int x = 1; x < xSize - 1; x++, vTop--, vMid++)
            {
                meshData.SetAsQuad(vMid, vTop, vMid + 1, vTop - 1);
            }

            meshData.SetAsQuad(vMid, vTop, vTop - 2, vTop - 1);
        }

        private static void CreateBottomFace(MeshData meshData, int xSize, int zSize, int ring, int vIndex)
        {
            int v = vIndex + 1;
            int vMid = meshData.vertices.Count - (xSize - 1) * (zSize - 1);
            meshData.SetAsQuad(vIndex + ring - 1, vIndex, vMid, vIndex + 1);
            for (int x = 1; x < xSize - 1; x++, v++, vMid++) 
                meshData.SetAsQuad(vMid, v, vMid + 1, v + 1);
            meshData.SetAsQuad(vMid, v, v + 2, v + 1);

            int vMin = vIndex + ring - 2;
            vMid -= xSize - 2;
            int vMax = v + 2;

            for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) 
            {
                meshData.SetAsQuad(vMin, vMin + 1, vMid + xSize - 1, vMid);
                for (int x = 1; x < xSize - 1; x++, vMid++)
                    meshData.SetAsQuad(vMid + xSize - 1, vMid, vMid + xSize, vMid + 1);
                meshData.SetAsQuad(vMid + xSize - 1, vMid, vMax + 1, vMax);
            }

            int vTop = vMin - 1;
            meshData.SetAsQuad(vTop + 1, vTop + 2, vTop, vMid);
            for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) 
                meshData.SetAsQuad(vTop, vMid, vTop - 1, vMid + 1);
            meshData.SetAsQuad(vTop, vMid, vTop - 1, vTop - 2);
        }
    }
}