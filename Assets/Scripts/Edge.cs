using UnityEngine;

namespace PCG_SearchBased_Dungeon
{
    public struct Edge
    {
        public Point Start;
        public Point End;

        public Edge(Point start, Point end)
        {
            Start = start;
            End = end;
        }
    }
}