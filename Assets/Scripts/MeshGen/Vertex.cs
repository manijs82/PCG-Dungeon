using System;
using UnityEngine;

namespace MeshGen
{
    public struct Vertex
    {
        public Vector3 position;
        public int triangle;

        public Vertex(Vector3 position, int triangle)
        {
            this.position = position;
            this.triangle = triangle;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Vertex other)
                return false;

            return position == other.position;
        }

        public bool Equals(Vertex other)
        {
            return position.Equals(other.position) && triangle == other.triangle;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(position, triangle);
        }
    }
}