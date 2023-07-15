using Mani;
using Mani.Geometry;
using Mani.Graph;
using UnityEditor;
using UnityEngine;

public static class MDSUtils
{
    
#if UNITY_EDITOR
    public static void DrawGizmos(this Line line)
    {
        Handles.color = Color.white;
        Handles.DrawLine(line.Origin.ToVector2(), (line.Origin + line.Direction).ToVector2(), 4);
        Handles.color = Color.white / 1.6f;
        Handles.DrawLine(line.GetPointAlong(-20).ToVector2(), line.GetPointAlong(20).ToVector2(), 4);
        Handles.color = Color.white;
    }
    
    public static void DrawGizmos(this Mani.Geometry.Triangle triangle)
    {
        Handles.color = Color.white;
        foreach (var lineSegment in TriangleUtils.GetLineSegments(triangle))
        {
            lineSegment.DrawGizmos();
        }
    }
    
    public static void DrawGizmos(this LineSegment lineSegment)
    {
        Handles.color = Color.white;
        Handles.DrawLine(lineSegment.Start.ToVector2(), lineSegment.End.ToVector2(), 3);
    }
    
    public static void DrawGizmos(this Circle circle, bool wired = false)
    {
        Handles.color = Color.white / 1.6f;
        if(wired)
            Handles.DrawWireDisc(circle.Center.ToVector2(), Vector3.back, circle.Radius, 4);
        else
            Handles.DrawSolidDisc(circle.Center.ToVector2(), Vector3.back, circle.Radius);
    }
    
    public static void DrawGizmos(this Point point)
    {
        Handles.color = Color.red;
        Handles.DrawSolidDisc(point.ToVector2(), Vector3.back, .2f);
    }
    
    public static void DrawGizmos(this Graph<Vector3> graph)
    {
        Handles.color = Color.red;
        foreach (var connection in graph.Edges)
        {
            Handles.DrawAAPolyLine(connection.Start.Value, connection.End.Value);
        }

        foreach (var node in graph.Nodes)
        {
            Handles.DrawSolidDisc(node.Value, Vector3.back, .5f);
            Handles.DrawSolidDisc(node.Value, Vector3.back, .5f);
        }
    }
#endif
    
    
    public static Vector2 ToVector2(this Point point)
    {
        return new Vector2(point.x, point.y);
    }

    public static Point ToPoint(this Vector2 vector2)
    {
        return new Point(vector2.x, vector2.y);
    }
    
    public static Point ToPoint(this Vector3 vector3)
    {
        return new Point(vector3.x, vector3.y);
    }
}