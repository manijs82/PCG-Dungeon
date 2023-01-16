using UnityEngine;

namespace PCG_SearchBased_Dungeon
{
    public class LinearEquation
    {
        public float A;
        public float B;
        public float C;
 
        public LinearEquation() { }
 
        //Ax+By=C
        public LinearEquation(Vector2 pointA, Vector2 pointB)
        {
            float deltaX = pointB.x - pointA.x;
            float deltaY = pointB.y - pointA.y;
            A = deltaY; //y2-y1
            B = -deltaX; //x1-x2
            C = A * pointA.x + B * pointA.y;
        }
 
        public LinearEquation PerpendicularLineAt(Vector3 point)
        {
            LinearEquation newLine = new LinearEquation();
 
            newLine.A = -B;
            newLine.B = A;
            newLine.C = newLine.A * point.x + newLine.B * point.y;
 
            return newLine;
        }
    }
}