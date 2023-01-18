using System;
using UnityEngine;

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
    }

    public Room(Room room) : this()
    {
        startPoint = room.startPoint;
        width = room.width;
        height = room.height;
    }

    public void SetCells()
    {
        cells = new CellType[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    cells[x, y] = CellType.Wall;
                else
                    cells[x, y] = CellType.Ground;
            }
        }
    }
}