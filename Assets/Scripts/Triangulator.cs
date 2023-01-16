using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PCG_SearchBased_Dungeon
{
    public static class Triangulator
    {
        public static IEnumerable<Triangle> Triangulate(List<Room> rooms)
        {
            var tri1 = new Triangle(new Point(0, 0), new Point(0, 50), new Point(50, 50));
            var tri2 = new Triangle(new Point(0, 0), new Point(50, 50), new Point(50, 0));
            var triangulation = new HashSet<Triangle>(new List<Triangle>{tri1, tri2});

            foreach (var room in rooms)
            {
                var point = new Point(room.Center);
                var invalids = FindBadTriangles(point, triangulation);
                var polygon = FindHoleBoundries(invalids);
                
                foreach (var triangle in invalids)
                {
                    foreach (var vertex in triangle.Vertices)
                    {
                        vertex.AdjacentTriangles.Remove(triangle);
                    }
                }
                triangulation.RemoveWhere(o => invalids.Contains(o));
                
                foreach (var edge in polygon.Where(possibleEdge => possibleEdge.Start != point && possibleEdge.End != point))
                {
                    var triangle = new Triangle(point, edge.Start, edge.End);
                    triangulation.Add(triangle);
                }
            }
            
            return triangulation;
        }

        private static List<Edge> FindHoleBoundries(ISet<Triangle> invalids)
        {
            var edges = new List<Edge>();
            foreach (var triangle in invalids)
            {
                edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
                edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
                edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
            }
            var boundaryEdges = edges.GroupBy(o => o).Where(o => o.Count() == 1).Select(o => o.First());
            return boundaryEdges.ToList();
        }
        
        private static ISet<Triangle> FindBadTriangles(Point point, HashSet<Triangle> triangles)
        {
            var badTriangles = triangles.Where(o => o.IsPointInsideCircumCircle(point));
            return new HashSet<Triangle>(badTriangles);
        }
    }
}