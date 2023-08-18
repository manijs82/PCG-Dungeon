using System.Collections.Generic;
using Freya;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public static class River
{
    public static void GenerateRiver(Dungeon dungeon, RiverProperties properties)
    {
        GameObject go = new GameObject("Spline");
        SplineContainer splineContainer = go.AddComponent<SplineContainer>();
        var spline = splineContainer.Spline;

        ConstructRiverSpline(dungeon, properties, spline);
        
        
    }

    private static void ConstructRiverSpline(Dungeon dungeon, RiverProperties properties, Spline spline)
    {
        Vector3 center = dungeon.endRoom.Center;
        
        List<Vector3> knotPoints = new List<Vector3>();
        for (int i = 0; i < properties.samplingDensity; i++)
        {
            float t = i / (float)properties.samplingDensity;
            Vector3 dir = Mathfs.AngToDir(Mathf.Lerp(0, Mathfs.TAU, t));
            float length = properties.distanceFromRoom +
                           Generator.dungeonRnd.Next(-properties.randomDistanceOffset, properties.randomDistanceOffset);
            Vector3 point = center + dir * length;
            if (Bound.Inside(point, dungeon.bound))
            {
                knotPoints.Add(point);
            }
        }

        for (int i = 0; i < knotPoints.Count; i++)
        {
            Vector3 point = knotPoints[i];
            var knot = new BezierKnot(new float3(point.x, point.y, point.z));
            spline.Add(knot, TangentMode.AutoSmooth);
        }
    }
}

[System.Serializable]
public struct RiverProperties
{
    public float thickness;
    public int distanceFromRoom;
    public int randomDistanceOffset;
    public int samplingDensity;
}