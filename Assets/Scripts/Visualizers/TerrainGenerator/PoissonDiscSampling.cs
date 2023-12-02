using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class PoissonDiscSampling
{
    public static List<Vector3> GeneratePoints(float radius, Bound sampleBound, int pointsLimit = Int32.MaxValue, int numSamplesBeforeRejection = 30)
    {
        List<Vector3> points = new List<Vector3>();
        
        float cellSize = radius / Mathf.Sqrt(2);
        Grid<SampleGridObject> grid = new Grid<SampleGridObject>(sampleBound.w, sampleBound.h,
            cellSize, (_, x, y) => new SampleGridObject(x, y, -1),
            new Vector3(sampleBound.x, sampleBound.y));

        Vector2 initialSamplePos = sampleBound.Center;
        points.Add(initialSamplePos);

        List<int> activeList = new List<int>() { 0 };
        
        grid.GetGridPosition(initialSamplePos, out int sampleX, out int sampleY);
        SampleGridObject initialSample = new SampleGridObject(sampleX, sampleY, 0);
        grid.SetValue(sampleX, sampleY, initialSample);
        
        while (activeList.Count != 0)
        {
            int pointIndex = activeList[Random.Range(0, activeList.Count)];
            Vector2 point = points[pointIndex];

            bool addedAnySample = false;
            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                Vector2 randomPointOnCircle = Random.insideUnitCircle * (radius * (Random.value + 1));
                Vector2 nextSamplePosition = point + randomPointOnCircle;

                if (IsValid(nextSamplePosition, radius, sampleBound, points, grid))
                {
                    points.Add(nextSamplePosition);
                    activeList.Add(points.Count - 1);

                    grid.GetGridPosition(nextSamplePosition, out int newX, out int newY);
                    SampleGridObject nextSample = new SampleGridObject(newX, newY, points.Count - 1);
                    grid.SetValue(newX, newY, nextSample);

                    addedAnySample = true;
                    break;
                }
            }

            if (!addedAnySample)
            {
                activeList.Remove(pointIndex);
            }
            
            if(points.Count > pointsLimit)
            {
                break;
            }
        }
        
        Debug.Log(points.Count);
        return points;
    }

    static bool IsValid(Vector2 candidate, float radius, Bound sampleBound, List<Vector3> points, Grid<SampleGridObject> grid)
    {
        if (!Bound.Inside(candidate, sampleBound))
            return false;
        
        grid.GetGridPosition(candidate, out int x, out int y);
        var neighbors = grid.GetNeighbors(x, y, new Bound(2, 2, 2, 2));
        foreach (var neighbor in neighbors)
        {
            if(neighbor.index == -1) continue;
            
            var neighborPos = points[neighbor.index];
            if (Vector2.Distance(candidate, neighborPos) >= radius)
                return true;
        }

        return false;
    }
}

public class SampleGridObject : GridObject
{
    public int index;
    
    public SampleGridObject(int x, int y, int index) : base(x, y)
    {
        this.index = index;
    }
}