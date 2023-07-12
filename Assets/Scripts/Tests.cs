using System;
using System.Collections.Generic;
using System.Linq;
using Mani;
using Mani.Geometry;
using Mani.Graph;
using UnityEditor;
using UnityEngine;
using Triangle = Mani.Geometry.Triangle;

public class Tests : MonoBehaviour
{
    [SerializeField] private Transform intersectionP1;
    [SerializeField] private Transform intersectionP2;
    [Space] [SerializeField] private Transform v1;
    [SerializeField] private Transform v2;
    [SerializeField] private Transform v3;
    [SerializeField] private Transform point;
    [Space] 
    [SerializeField] private List<Transform> nodes;
    [Space] 
    [SerializeField] private Transform lv1;
    [SerializeField] private Transform lv2;
    [SerializeField] private Transform lv3;
    [SerializeField] private Transform lv4;
    


    private void OnDrawGizmos()
    {
        IntersectionTest();

        CircumCircleTest();

        TriangulationTest();

        if(lv1 == null || lv2 == null || lv3 == null || lv4 == null) return;
        List<Mani.Geometry.Triangle> tris = new List<Mani.Geometry.Triangle>()
        {
            new(lv1.position.ToPoint(), lv2.position.ToPoint(), lv3.position.ToPoint()),
            new(lv3.position.ToPoint(), lv4.position.ToPoint(), lv1.position.ToPoint()),
        };
        foreach (var edge in TriangleUtils.GetUniqueEdges(tris))
        {
            edge.DrawGizmos();
        }
    }

    private Func<Node<Vector3>, Point> posFunc;
    private Graph<Vector3> graph;
    private Mani.Geometry.Triangle superTriangle;
    private List<Mani.Geometry.Triangle> triangles;
    private List<Point> points;
    private int lastIndex;

    private void Start()
    {
        if (nodes == null) return;
        if (nodes.Count <= 3) return;
        Graph<Vector3> g = new Graph<Vector3>();
        graph = g;
        foreach (var node in nodes)
        {
            graph.AddNode(new Node<Vector3>(node.position));
        }

        TriangulateDelaunay(node => node.Value.ToPoint());
    }

    private void TriangulationTest()
    {
        Graph<Vector3> g = new Graph<Vector3>();
        foreach (var node in nodes) g.AddNode(new Node<Vector3>(node.position));
        g.TriangulateDelaunay(node => node.Value.ToPoint());
        g.DrawGizmos();

        if(triangles == null) return;
        superTriangle.DrawGizmos();
        foreach (var triangle in triangles)
        {
            triangle.DrawGizmos();
            triangle.CircumCircle.DrawGizmos(true);
        }
        //graph.DrawGizmos();
    }

    private void TriangulateDelaunay(Func<Node<Vector3>, Point> getNodePos)
    {
        posFunc = getNodePos;
        points = new List<Point>();
        foreach (var node in graph.Nodes) points.Add(getNodePos(node));

        // initialise with super triangle
        superTriangle = TriangleUtils.GetSuperTriangle(points);
        triangles = new List<Mani.Geometry.Triangle>
        {
            superTriangle
        };
        
    }

    [ContextMenu("Move")]
    private void Move()
    {
        if(lastIndex == points.Count)
        {
            var trianglesConnectedToSuperTriangle =
                new List<Mani.Geometry.Triangle>(triangles.Where(t => TriangleUtils.ShareVertex(t, superTriangle)));
            foreach (var triangle in trianglesConnectedToSuperTriangle)
            {
                triangles.Remove(triangle);
            }

            foreach (var triangle in triangles)
            {
                AddTriangleToGraph(triangle, posFunc);
            }

            lastIndex++;
            return;
        }
        
        var p = points[lastIndex];
        var invalidTriangles = new List<Mani.Geometry.Triangle>(triangles.Where(t => t.IsPointInsideCircumCircle(p)));
        var connectingLines = new List<LineSegment>();
        foreach (var triangle in invalidTriangles)
        {
            connectingLines.AddRange(TriangleUtils.GetUniqueEdges(invalidTriangles));
            triangles.Remove(triangle);
        }

        foreach (var line in connectingLines)
        {
            triangles.Add(new Mani.Geometry.Triangle(line.Start, line.End, p));
        }

        lastIndex++;
    }

    private void AddTriangleToGraph(Mani.Geometry.Triangle triangle, Func<Node<Vector3>, Point> getNodePos,
        bool setCosts = true)
    {
        var node0 = graph.Nodes.FirstOrDefault(n => getNodePos(n) == triangle.Vertex0);
        var node1 = graph.Nodes.FirstOrDefault(n => getNodePos(n) == triangle.Vertex1);
        var node2 = graph.Nodes.FirstOrDefault(n => getNodePos(n) == triangle.Vertex2);

        graph.AddEdge(node0, node1, setCosts ? Point.Distance(getNodePos(node0), getNodePos(node1)) : 0);
        graph.AddEdge(node1, node2, setCosts ? Point.Distance(getNodePos(node1), getNodePos(node2)) : 0);
        graph.AddEdge(node1, node0, setCosts ? Point.Distance(getNodePos(node2), getNodePos(node0)) : 0);
    }

    private void CircumCircleTest()
    {
        if (v1 == null || v2 == null || v3 == null || point == null) return;

        Mani.Geometry.Triangle triangle =
            new Mani.Geometry.Triangle(v1.position.ToPoint(), v2.position.ToPoint(), v3.position.ToPoint());
        triangle.DrawGizmos();
        triangle.CircumCircle.DrawGizmos();
        if (triangle.IsPointInsideCircumCircle(point.position.ToPoint()))
        {
            point.position.ToPoint().DrawGizmos();
        }
    }

    private void IntersectionTest()
    {
        if (intersectionP1 == null || intersectionP2 == null) return;

        Line line1 = new Line(intersectionP1.position.ToPoint(), intersectionP1.right.ToPoint());
        Line line2 = new Line(intersectionP2.position.ToPoint(), intersectionP2.right.ToPoint());

        line1.DrawGizmos();
        line2.DrawGizmos();

        if (Line.LineLineIntersection(line1, line2, out Point point))
        {
            point.DrawGizmos();
        }
    }
}