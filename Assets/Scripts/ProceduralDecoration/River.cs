using System.Collections.Generic;
using Freya;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class River : GridDecorator
{
    private Dungeon dungeon;
    private Spline riverSpline;
    private RiverProperties riverProperties;

    public River(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        riverProperties = this.dungeon.dungeonParameters.riverProperties;
    }
    
    public override void Decorate(Grid<GridObject> grid)
    {
        GenerateRiverSpline();
        dungeon.RemoveRoomsCollidingWithSpline(riverSpline, riverProperties.thickness);
    }

    public void GenerateRiverSpline()
    {
        GameObject go = new GameObject("Spline");
        SplineContainer splineContainer = go.AddComponent<SplineContainer>();
        Spline spline = splineContainer.Spline;

        Vector3 center = dungeon.endRoom.Center;
        
        List<Vector3> knotPoints = new List<Vector3>();
        for (int i = 0; i < riverProperties.samplingDensity; i++)
        {
            float t = i / (float)riverProperties.samplingDensity;
            Vector3 dir = Mathfs.AngToDir(Mathf.Lerp(0, Mathfs.TAU, t));
            float length = riverProperties.distanceFromRoom +
                           Generator.dungeonRnd.Next(-riverProperties.randomDistanceOffset, riverProperties.randomDistanceOffset);
            Vector3 point = center + dir * length;
            if (Bound.Inside(point, dungeon.bound))
            {
                knotPoints.Add(point);
            }
        }

        foreach (var point in knotPoints)
        {
            var knot = new BezierKnot(new float3(point.x, point.y, point.z));
            spline.Add(knot, TangentMode.AutoSmooth);
        }

        riverSpline = spline;
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