using UnityEngine;

public struct Edge
{
    public Vector2 Start;
    public Vector2 End;

    public Edge(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
    }
}