using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PCG_SearchBased_Dungeon
{
    public static class Triangulator
    {
        public static List<Triangle> Triangulate(List<Room> rooms)
        {
            var triangles = new List<Triangle>();
            var superTris = GetSuperTriangle();
            triangles.Add(superTris);

            foreach (var room in rooms)
            {
                var point = room.Center;
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
            
            var outerTris = new List<Triangle>(triangles.Where(t => ShareEdgeWithSuperTriangle(superTris, t)));
            foreach (var triangle in outerTris) 
                triangles.Remove(triangle);

            return triangles;
        }

        private static bool ShareEdgeWithSuperTriangle(Triangle superTris, Triangle tris)
        {
            return tris.Vertices[0] == superTris.Vertices[0] || tris.Vertices[0] == superTris.Vertices[1] ||
                   tris.Vertices[0] == superTris.Vertices[2] ||
                   tris.Vertices[1] == superTris.Vertices[0] || tris.Vertices[1] == superTris.Vertices[1] ||
                   tris.Vertices[1] == superTris.Vertices[2] ||
                   tris.Vertices[2] == superTris.Vertices[0] || tris.Vertices[2] == superTris.Vertices[1] ||
                   tris.Vertices[2] == superTris.Vertices[2];
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
        
        private static List<Triangle> FindBadTriangles(Vector2 point, List<Triangle> triangles)
        {
            return triangles.Where(o => o.IsPointInsideCircumCircle(point)).ToList();
        }
    }
}