﻿using MeshGen;
using UnityEngine;

namespace Utils
{
    public static class MeshUtils
    {
        public static void AddCube(this MeshData meshData, Vector3 center, Vector3 halfExtents)
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
        
        public static void AddQuad(this MeshData meshData, Vector3 center, float halfWidth, float halfHeight, Vector3 right, Vector3 up)
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
    }
}