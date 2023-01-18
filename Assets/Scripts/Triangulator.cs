﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Triangulator
{
    public static List<Triangle> Triangulate(List<Room> rooms)
    {
        var triangles = new List<Triangle>();
        var superTris = GetSuperTriangle();
        triangles.Add(superTris);

        foreach (var room in rooms) 
            TriangulatePoint(room.Center, triangles);

        var outerTris = new List<Triangle>(triangles.Where(t => ShareEdges(superTris, t)));
        foreach (var triangle in outerTris) 
            triangles.Remove(triangle);

        return triangles;
    }

    private static void TriangulatePoint(Vector2 point, List<Triangle> triangles)
    {
        var edges = new List<Edge>();
        var invalidTris = new List<Triangle>(triangles.Where(t => t.IsPointInsideCircumCircle(point)));

        foreach (var triangle in invalidTris)
        {
            edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
            edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
            edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
            triangles.Remove(triangle);
        }

        edges = RemoveDuplicate(edges);

        foreach (var edge in edges)
            triangles.Add(new Triangle(edge.Start, edge.End, point));
    }

    private static bool ShareEdges(Triangle tri1, Triangle tri2)
    {
        return tri2.Vertices[0] == tri1.Vertices[0] || tri2.Vertices[0] == tri1.Vertices[1] ||
               tri2.Vertices[0] == tri1.Vertices[2] ||
               tri2.Vertices[1] == tri1.Vertices[0] || tri2.Vertices[1] == tri1.Vertices[1] ||
               tri2.Vertices[1] == tri1.Vertices[2] ||
               tri2.Vertices[2] == tri1.Vertices[0] || tri2.Vertices[2] == tri1.Vertices[1] ||
               tri2.Vertices[2] == tri1.Vertices[2];
    }

    private static List<Edge> RemoveDuplicate(List<Edge> edges)
    {
        var o = new List<Edge>();
        for (var i = 0; i < edges.Count; ++i) {
            var isUnique = true;

            for (var j = 0; j < edges.Count; ++j) {
                if (i != j && AreTheSameEdge(edges[i], edges[j])) {
                    isUnique = false;
                    break;
                }
            }

            if(isUnique) o.Add(edges[i]);
        }

        return o;
    }

    private static bool AreTheSameEdge(Edge e1, Edge e2)
    {
        return (e1.Start == e2.Start && e1.End == e2.End) ||
               (e1.End == e2.Start && e1.Start == e2.End);
    }

    private static Triangle GetSuperTriangle()
    {
        float minx = 0;
        float maxx = 50;
        float miny = 0;
        float maxy = 50;
            
        var dx = (maxx - minx) * 10;
        var dy = (maxy - miny) * 10;

        var v0 = new Vector2(minx - dx, miny - dy * 3);
        var v1 = new Vector2(minx - dx, maxy + dy);
        var v2 = new Vector2(maxx + dx * 3, maxy + dy);

        return new Triangle(v0, v1, v2);
    }
}