namespace MeshGen
{
    public struct Triangle
    {
        public int vertex1;
        public int vertex2;
        public int vertex3;

        public int adjacentTriangle1;
        public int adjacentTriangle2;
        public int adjacentTriangle3;

        public Triangle(int vertex1, int vertex2, int vertex3, int adjacentTriangle1, int adjacentTriangle2, int adjacentTriangle3)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.vertex3 = vertex3;
            this.adjacentTriangle1 = adjacentTriangle1;
            this.adjacentTriangle2 = adjacentTriangle2;
            this.adjacentTriangle3 = adjacentTriangle3;
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

        public bool Contains(int vIndex)
        {
            return vertex1 == vIndex || vertex2 == vIndex || vertex3 == vIndex;
        }
        
        public void SetAdjacentTriangle(int vIndex1, int vIndex2, int adjacentTriangleIndex)
        {
            if (vertex1 == vIndex1 && vertex2 == vIndex2)
            {
                adjacentTriangle1 = adjacentTriangleIndex;
            }
            
            if (vertex2 == vIndex1 && vertex3 == vIndex2)
            {
                adjacentTriangle2 = adjacentTriangleIndex;
            }
            
            if (vertex3 == vIndex1 && vertex1 == vIndex2)
            {
                adjacentTriangle3 = adjacentTriangleIndex;
            }
        }
    }
}