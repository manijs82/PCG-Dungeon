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
    
    private static GameObject splineGameObject;

    public River(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        riverProperties = this.dungeon.dungeonParameters.riverProperties;
    }
    
    public override void Decorate(Grid<GridObject> grid)
    {
        GenerateRiverSpline();
        dungeon.RemoveRoomsCollidingWithSpline(riverSpline, riverProperties.thickness);
        
        int division = (int) riverSpline.GetLength();
        for (int i = 0; i < division; i++)
        {
            float t = i / (division - 1f);
            var pos = riverSpline.EvaluatePosition(t);

            grid.GetGridPosition(pos, out int x, out int y);
            grid.SetValue(x, y, new Bound(riverProperties.thickness, riverProperties.thickness, riverProperties.thickness, riverProperties.thickness),
                (xPos, yPos) => new RiverTileObject(xPos, yPos, CellType.Empty));
        }
    }

    public void GenerateRiverSpline()
    {
        if (splineGameObject != null)
        {
            Object.Destroy(splineGameObject);
        }
        
        GameObject go = new GameObject("Spline");
        SplineContainer splineContainer = go.AddComponent<SplineContainer>();
        Spline spline = splineContainer.Spline;

        Vector3 center = dungeon.GetRandomCornerRoom().Value.Center;
        
        List<Vector3> knotPoints = new List<Vector3>();
        for (int i = 0; i < riverProperties.samplingDensity; i++)
        {
            float t = i / (float)riverProperties.samplingDensity;
            Vector3 dir = Mathfs.AngToDir(Mathf.Lerp(0, Mathfs.TAU, t));
            float length = riverProperties.distanceFromRoom;
            float offset = Generator.dungeonRnd.Next(-riverProperties.randomDistanceOffset,
                riverProperties.randomDistanceOffset);
            length += offset;
            Vector3 point = center + dir * length;
            knotPoints.Add(point);
        }

        foreach (var point in knotPoints)
        {
            var knot = new BezierKnot(new float3(point.x, point.y, point.z));
            spline.Add(knot, TangentMode.AutoSmooth);
        }

        spline.Closed = true;
        riverSpline = spline;
        splineGameObject = go;
    }
}

[System.Serializable]
public struct RiverProperties
{
    public int thickness;
    public int distanceFromRoom;
    public int randomDistanceOffset;
    public int samplingDensity;
}