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
    }
}