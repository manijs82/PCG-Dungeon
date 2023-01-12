using System;
using UnityEngine;

namespace PCG_SearchBased_Dungeon
{
    public struct Corridor
    {
        private Vector2 startPoint;
        private int length;
        private bool isHorizontal;
        private CellType[,] cells;

        public Corridor(Vector2 startPoint, int length, bool isHorizontal) : this()
        {
            this.startPoint = startPoint;
            this.length = length;
            this.isHorizontal = isHorizontal;
            cells = isHorizontal ? new CellType[length, 3] : new CellType[3, length];
            SetCells();
        }

        private void SetCells()
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    if (!isHorizontal && (x == 0 || x == cells.GetLength(0) - 1)) 
                        cells[x, y] = CellType.Wall;
                    if (isHorizontal && (y == 0 || y == cells.GetLength(1) - 1)) 
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