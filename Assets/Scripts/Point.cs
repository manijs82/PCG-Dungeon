using System.Collections.Generic;
using UnityEngine;

namespace PCG_SearchBased_Dungeon
{
    public class Point
    {
        public Vector2 point;

        public HashSet<Triangle> AdjacentTriangles { get; } = new HashSet<Triangle>();
        public float x => point.x;
        public float y => point.y;

        public Point(Vector2 point)
        {
            this.point = point;
        }
        
        public Point(float x, float y)
        {
            this.point = new Vector2(x, y);
        }
    }
}