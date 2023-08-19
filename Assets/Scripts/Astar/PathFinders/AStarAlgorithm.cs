using UnityEngine;

public class AStarAlgorithm : DijkstrasAlgorithm
{
    public AStarAlgorithm(Grid<GridObject> pathGrid, GridObject start, GridObject goal) : base(pathGrid, start, goal)
    {
    }

    protected override float GetPriority(float costSoFar, Vector2 nextPos)
    {
        return costSoFar + GetDist(goal.Point, nextPos);
    }

    private float GetDist(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }
    
    private float GetManhattanDist(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}