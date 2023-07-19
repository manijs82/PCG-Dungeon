using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstrasAlgorithm : BreadthFirstSearch
{

    protected Dictionary<GridObject, float> frontiersPriorityQueue;
    protected Dictionary<GridObject, float> costs;

    public DijkstrasAlgorithm(Grid<GridObject> pathGrid, GridObject start, GridObject goal) : base(pathGrid, start, goal)
    {
    }

    public override void PathFindingSearch()
    {
        frontiersPriorityQueue = new Dictionary<GridObject, float>();
        frontiersPriorityQueue.Add(start, 0);
        searched = new Dictionary<GridObject, GridObject>();
        searched.Add(start, start);
        costs = new Dictionary<GridObject, float>();
        costs.Add(start, 0);

        while (frontiersPriorityQueue.Count > 0)
        {
            //yield return new WaitForSeconds(.03f);
            GridObject current = GetHighestPriority();
            frontiersPriorityQueue.Remove(current);
            
            if(current == goal)
                break;

            foreach (var gridObj in pathGrid.Get4Neighbors(current))
            {
                float nextCost = costs[current] + current.Cost + gridObj.Cost;
                if (!costs.ContainsKey(gridObj) || nextCost < costs[gridObj])
                {
                    if(costs.ContainsKey(gridObj))
                        costs[gridObj] = nextCost;
                    else    
                        costs.Add(gridObj, nextCost);
                    if(frontiersPriorityQueue.ContainsKey(gridObj))
                        frontiersPriorityQueue[gridObj] = GetPriority(nextCost, gridObj.Point);
                    else    
                        frontiersPriorityQueue.Add(gridObj, GetPriority(nextCost, gridObj.Point));
                    if(searched.ContainsKey(gridObj))
                        searched[gridObj] = current;
                    else    
                        searched.Add(gridObj, current);
                }
            }
        }
        
        SetPath();
    }

    protected virtual float GetPriority(float costSoFar, Vector2 nextPos) => costSoFar;

    protected GridObject GetHighestPriority()
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
}