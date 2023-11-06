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

        public bool Equals(Vector3 other)
        {
            return position.Equals(other);
        }

        public bool Equals(Vertex other)
        {
            return position.Equals(other.position);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }
}