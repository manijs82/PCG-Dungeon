using UnityEngine;

namespace PCG_SearchBased_Dungeon
{
    public static class CircumCenter
    {
        public static Vector2 GetCircumCenter(Vector2 pointA, Vector2 pointB, Vector2 pointC, out float radius)
        {
            LinearEquation lineAB = new LinearEquation(pointA, pointB);
            LinearEquation lineBC = new LinearEquation(pointB, pointC);
 
            Vector2 midPointAB = Vector2.Lerp(pointA, pointB, .5f);
            Vector2 midPointBC = Vector2.Lerp(pointB, pointC, .5f);
 
            LinearEquation perpendicularAB = lineAB.PerpendicularLineAt(midPointAB);
            LinearEquation perpendicularBC = lineBC.PerpendicularLineAt(midPointBC);
 
            Vector2 center = GetCrossingPoint(perpendicularAB, perpendicularBC);
            
            radius = Vector2.Distance(center, pointA);
            return center;
        }
 
        static Vector2 GetCrossingPoint(LinearEquation line1, LinearEquation line2)
        {
            float A1 = line1.A;
            float A2 = line2.A;
            float B1 = line1.B;
            float B2 = line2.B;
            float C1 = line1.C;
            float C2 = line2.C;
 
            //Cramer's rule
            float Determinant = A1 * B2 - A2 * B1;
            float DeterminantX = C1 * B2 - C2 * B1;
            float DeterminantY = A1 * C2 - A2 * C1;
 
            float x = DeterminantX / Determinant;
            float y = DeterminantY / Determinant;
 
            return new Vector2(x, y);
        }
    }
}