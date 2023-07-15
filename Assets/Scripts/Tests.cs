using System.Collections.Generic;
using System.Linq;
using Mani;
using Mani.Geometry;
using Mani.Graph;
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

        UniqueEdgeTest();
    }

    private void UniqueEdgeTest()
    {
        if (lv1 == null || lv2 == null || lv3 == null || lv4 == null) return;
        List<Triangle> tris = new List<Triangle>()
        {
            new(lv1.position.ToPoint(), lv2.position.ToPoint(), lv3.position.ToPoint()),
            new(lv3.position.ToPoint(), lv4.position.ToPoint(), lv1.position.ToPoint()),
        };
        foreach (var edge in TriangleUtils.GetUniqueEdges(tris))
        {
            edge.DrawGizmos();
        }
    }

    private void TriangulationTest()
    {
        Graph<Vector3> g = new Graph<Vector3>();
        foreach (var node in nodes) g.AddNode(new Node<Vector3>(node.position));
        g.TriangulateDelaunay(node => node.Value.ToPoint());
        g = GetPrimsMinimumSpanningTree(g);
        g.DrawGizmos();
    }
    
    public Graph<Vector3> GetPrimsMinimumSpanningTree(Graph<Vector3> graph)
    {
        var mst = new Graph<Vector3>();

        // Initialize tree with arbitrary start node
        var start = graph.Nodes[0];
        var includedVertices = new List<Node<Vector3>>();
        var nextEdges = new List<Edge<Vector3>>();
        includedVertices.Add(start);
        nextEdges.AddRange(graph.GetAllConnectedEdges(start));

        while (mst.NodeCount != nodes.Count) // loop until all nodes are included
        {
            // find lowest cost next edge
            float lowestCost = float.PositiveInfinity;
            Edge<Vector3> lowestCostEdge = null;
            foreach (var edge in nextEdges)
            {
                if (edge.Cost > lowestCost) continue;
                lowestCost = edge.Cost;
                lowestCostEdge = edge;
            }

            includedVertices.Add(lowestCostEdge.Start);
            includedVertices.Add(lowestCostEdge.End);
            mst.AddEdge(lowestCostEdge);
            nextEdges.Remove(lowestCostEdge);

            foreach (var edge in graph.GetAllConnectedEdges(lowestCostEdge))
            {
                if(nextEdges.Contains(edge)) continue;
                if(mst.ContainEdge(edge)) continue;
                if(mst.Nodes.Contains(edge.Start) && mst.Nodes.Contains(edge.End)) continue;
                nextEdges.Add(edge);
            }
        }

        return mst;
    }

    private void CircumCircleTest()
    {
        if (v1 == null || v2 == null || v3 == null || point == null) return;

        Triangle triangle =
            new Triangle(v1.position.ToPoint(), v2.position.ToPoint(), v3.position.ToPoint());
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