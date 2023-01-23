using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Grid = HyperCasual_Engine.Utils.Grid;

public class BreadthFirstSearch
{
    public event Action<List<GridObject>> OnPathCalculated;

    public List<GridObject> path;
    
    protected Grid<GridObject> pathGrid;
    protected GridObject start;
    protected GridObject goal;
    protected Queue<GridObject> frontiers;
    protected Dictionary<GridObject, GridObject> searched;

    public BreadthFirstSearch(Grid<GridObject> pathGrid, GridObject start, GridObject goal)
    {
        this.pathGrid = pathGrid;
        this.start = start;
        this.goal = goal;
    }

    public virtual void PathFindingSearch()
    {
        frontiers = new Queue<GridObject>();
        frontiers.Enqueue(start);
        searched = new Dictionary<GridObject, GridObject>();
        searched.Add(start, default);

        while (frontiers.Count > 0)
        {
            GridObject current = frontiers.Dequeue();
            if(current == goal)
                break;

            foreach (var gridObj in pathGrid.GetNeighbors(current))
            {
                //yield return null;
                if (!searched.ContainsKey(gridObj))
                {
                    frontiers.Enqueue(gridObj);
                    searched.Add(gridObj, current);
                }
            }
        }

        SetPath();
    }


    protected void SetPath()
    {
        GridObject currentPlace = goal;
        path = new List<GridObject>();
        
        while (currentPlace != start)
        {
            path.Add(currentPlace);
            currentPlace = searched[currentPlace];
        }
        path.Add(start);

        OnPathCalculated?.Invoke(path);
    }
}