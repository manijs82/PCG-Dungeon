using System;

namespace MeshGen
{
    public struct Triangle
    {
        public int vertex1;
        public int vertex2;
        public int vertex3;
        
        public int index;

        public int adjacentTriangle1;
        public int adjacentTriangle2;
        public int adjacentTriangle3;

        private int[] vertices;

        public Triangle(int vertex1, int vertex2, int vertex3, int index, int adjacentTriangle1, int adjacentTriangle2, int adjacentTriangle3)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.vertex3 = vertex3;
            this.index = index;
            this.adjacentTriangle1 = adjacentTriangle1;
            this.adjacentTriangle2 = adjacentTriangle2;
            this.adjacentTriangle3 = adjacentTriangle3;

            vertices = new[] { vertex1, vertex2, vertex3 };
        }
        
        public Triangle(int vertex1, int vertex2, int vertex3, int index)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.vertex3 = vertex3;
            this.index = index;
            this.adjacentTriangle1 = -1;
            this.adjacentTriangle2 = -1;
            this.adjacentTriangle3 = -1;
            
            vertices = new[] { vertex1, vertex2, vertex3 };
        }

        public int[] GetEdgeVertices(int edgeIndex)
        {
            switch (edgeIndex)
            {
                case 0:
                    return new[] { vertex1, vertex2 };
                case 1:
                    return new[] { vertex2, vertex3 };
                case 2:
                    return new[] { vertex3, vertex1 };
            }

            return null;
        }
        
        public int GetEdgeIndex(int vIndex1, int vIndex2)
        {
            if ((vertex1 == vIndex1 && vertex2 == vIndex2) || (vertex2 == vIndex1 && vertex1 == vIndex2))
            {
                return 0;
            }
            
            if ((vertex2 == vIndex1 && vertex3 == vIndex2) || (vertex3 == vIndex1 && vertex2 == vIndex2))
            {
                return 1;
            }
            
            if ((vertex3 == vIndex1 && vertex1 == vIndex2) || (vertex1 == vIndex1 && vertex3 == vIndex2))
            {
                return 2;
            }

            return -1;
        }
        
        public int GetEdgeTriangleIndex(int edgeIndex)
        {
            switch (edgeIndex)
            {
                case 0:
                    return adjacentTriangle1;
                case 1:
                    return adjacentTriangle2;
                case 2:
                    return adjacentTriangle3;
            }

            return -1;
        }

        public int GetOtherVertex(int v1, int v2)
        {
            if ((v1 == vertex1 && v2 == vertex2) || (v1 == vertex2 && v2 == vertex1))
                return vertex3;
            if ((v1 == vertex2 && v2 == vertex3) || (v1 == vertex3 && v2 == vertex2))
                return vertex1;
            if ((v1 == vertex1 && v2 == vertex3) || (v1 == vertex3 && v2 == vertex1))
                return vertex2;
            return -1;
        }
        
        public int GetVertexIndex(int vertex)
        {
            if (vertex1 == vertex)
                return 0;
            if (vertex2 == vertex)
                return 1;
            if (vertex3 == vertex)
                return 2;
            
            return -1;
        }

        public bool IsAdjacentTo(Triangle other)
        {
            int sharedVertices = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (vertices[i] == other.vertices[j])
                    {
                        sharedVertices++;
                        if (sharedVertices == 2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsAdjacentTo(Triangle other, out int v1, out int v2)
        {
            int sharedVertices = 0;
            v1 = -1;
            v2 = -1;
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var v = vertices[i];
                    if (v == other.vertices[j])
                    {
                        sharedVertices++;
                        if (sharedVertices == 1)
                        {
                            v1 = v;
                        }
                        if (sharedVertices == 2)
                        {
                            v2 = v;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void SetAdjacentTriangle(int edgeIndex, int adjacentTriangleIndex)
        {
            switch (edgeIndex)
            {
                case 0:
                    adjacentTriangle1 = adjacentTriangleIndex;
                    break;
                case 1:
                    adjacentTriangle2 = adjacentTriangleIndex;
                    break;
                case 2:
                    adjacentTriangle3 = adjacentTriangleIndex;
                    break;
            }
        }

        public void SetAdjacentTriangle(int vIndex1, int vIndex2, int adjacentTriangleIndex)
        {
            if ((vertex1 == vIndex1 && vertex2 == vIndex2) || (vertex2 == vIndex1 && vertex1 == vIndex2))
            {
                adjacentTriangle1 = adjacentTriangleIndex;
            }
            
            if ((vertex2 == vIndex1 && vertex3 == vIndex2) || (vertex3 == vIndex1 && vertex2 == vIndex2))
            {
                adjacentTriangle2 = adjacentTriangleIndex;
            }
            
            if ((vertex3 == vIndex1 && vertex1 == vIndex2) || (vertex1 == vIndex1 && vertex3 == vIndex2))
            {
                adjacentTriangle3 = adjacentTriangleIndex;
            }
        }

        public bool Contains(int vIndex)
        {
            return vertex1 == vIndex || vertex2 == vIndex || vertex3 == vIndex;
        }

        public static bool operator ==(Triangle a, Triangle b) => a.Equals(b);
        
        public static bool operator !=(Triangle a, Triangle b) => !a.Equals(b);
        
        public bool Equals(Triangle other)
        {
            return vertex1 == other.vertex1 && vertex2 == other.vertex2 && vertex3 == other.vertex3;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(vertex1, vertex2, vertex3);
        }
    }
}