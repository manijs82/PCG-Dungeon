using System.Collections.Generic;
using UnityEngine;

namespace PCG_SearchBased_Dungeon
{
    public class Triangle
    {
        public Vector2[] Vertices = new Vector2[3];
        public List<Edge> Edges = new List<Edge>();

        public Vector2 CircumCenter;
        public double RadiusSquared;

        public Triangle(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            if (!IsCounterClockwise(point1, point2, point3))
            {
                Vertices[0] = point1;
                Vertices[1] = point3;
                Vertices[2] = point2;
            }
            else
            {
                Vertices[0] = point1;
                Vertices[1] = point2;
                Vertices[2] = point3;
            }
            
            SetCircumCenter();
        }
        
        private bool IsCounterClockwise(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            var result = (point2.x - point1.x) * (point3.y - point1.y) -
                         (point3.x - point1.x) * (point2.y - point1.y);
            return result > 0;
        }

        public void SetCircumCenter()
        {
            var p0 = Vertices[0];
            var p1 = Vertices[1];
            var p2 = Vertices[2];
            var dA = p0.x * p0.x + p0.y * p0.y;
            var dB = p1.x * p1.x + p1.y * p1.y;
            var dC = p2.x * p2.x + p2.y * p2.y;

            var aux1 = (dA * (p2.y - p1.y) + dB * (p0.y - p2.y) + dC * (p1.y - p0.y));
            var aux2 = -(dA * (p2.x - p1.x) + dB * (p0.x - p2.x) + dC * (p1.x - p0.x));
            var div = (2 * (p0.x * (p2.y - p1.y) + p1.x * (p0.y - p2.y) + p2.x * (p1.y - p0.y)));

            if (div == 0)
            {
                return;
            }

            var center = new Vector2(aux1 / div, aux2 / div);
            CircumCenter = center;
            RadiusSquared = (center.x - p0.x) * (center.x - p0.x) + (center.y - p0.y) * (center.y - p0.y);
        }

        public bool IsPointInsideCircumCircle(Vector2 point)
        {
            var d_squared = (point.x - CircumCenter.x) * (point.x - CircumCenter.x) +
                            (point.y - CircumCenter.y) * (point.y - CircumCenter.y);
            return d_squared < RadiusSquared;
        }
    }
}