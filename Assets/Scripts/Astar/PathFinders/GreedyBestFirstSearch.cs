using System.Collections.Generic;
using UnityEngine;

public class GreedyBestFirstSearch : BreadthFirstSearch
{
    
    private Dictionary<GridObject, float> frontiersPriorityQueue;

    public GreedyBestFirstSearch(Grid<GridObject> pathGrid, GridObject start, GridObject goal) : base(pathGrid, start, goal)
    {
    }

    public override void PathFindingSearch()
    {
        frontiersPriorityQueue = new Dictionary<GridObject, float>();
        frontiersPriorityQueue.Add(start, 0);
        searched = new Dictionary<GridObject, GridObject>();
        searched.Add(start, start);

        while (frontiersPriorityQueue.Count > 0)
        {
            //yield return new WaitForSeconds(.03f);
            GridObject current = GetHighestPriority();
            frontiersPriorityQueue.Remove(current);
            if(current == goal)
                break;
            
            foreach (var gridObj in pathGrid.Get4Neighbors(current))
            {
                
                // yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.D));
                if (!searched.ContainsKey(gridObj))
                {
                    frontiersPriorityQueue.Add(gridObj, GetDist(goal.Point, gridObj.Point));
                    searched.Add(gridObj, current);
                }
            }
        }

        SetPath();
    }

    private GridObject GetHighestPriority()
    {
        GridObject o = null;
        float lowestKey = 10000;
        foreach (var gridObject in frontiersPriorityQueue)
        {
            if (gridObject.Value < lowestKey)
            {
                lowestKey = gridObject.Value;
                o = gridObject.Key;
            }
        }

        return o;
    }

    private float GetDist(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }
}