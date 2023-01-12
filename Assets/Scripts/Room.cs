using System;
using UnityEngine;

namespace PCG_SearchBased_Dungeon
{
    public struct Room
    {
        public CellType[,] cells;
        public Vector2 startPoint;
        private int width;
        private int height;

        public Vector2 Center => new(startPoint.x + width / 2f, startPoint.y + height / 2f);
        public Bound Bound => new ((int)startPoint.x, (int)startPoint.y, width, height);

        public Room(Vector2 startPoint, int width, int height) : this()
        {
            this.startPoint = startPoint;
            this.width = width;
            this.height = height;
            cells = new CellType[width, height];
        }

        public Room(Room room) : this()
        {
            startPoint = room.startPoint;
            width = room.width;
            height = room.height;
            cells = new CellType[width, height];
        }

        public void SetCells()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        cells[x, y] = CellType.Wall;
                }
            }
        }

        public override string ToString()
        {
            string o = String.Empty;

            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    o += Dungeon.GetChar(cells[x, y]);
                }

                o += "\n";
            }

            return o;
        }
    }
}