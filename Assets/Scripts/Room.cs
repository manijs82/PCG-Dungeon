using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Grid<GridObject> grid;
    public Vector2 startPoint;
    public List<Vector2> doors;
    public Bound bound;

    private int width;
    private int height;

    public Vector2 Center => bound.Center;

    public Room(Vector2 startPoint, int width, int height)
    {
        this.startPoint = startPoint;
        this.width = width;
        this.height = height;
        doors = new List<Vector2>();
        bound = new Bound((int)startPoint.x, (int)startPoint.y, width, height);
    }

    public Room(Room room)
    {
        startPoint = room.startPoint;
        width = room.width;
        height = room.height;
        doors = new List<Vector2>();
        bound = new Bound((int)startPoint.x, (int)startPoint.y, width, height);
    }

    public void ChangePosition(Vector2 newPos)
    {
        startPoint = newPos;
        bound.x = (int)newPos.x;
        bound.y = (int)newPos.y;
    }

    public void InitializeGrid()
    {
        grid = new Grid<GridObject>(bound.w, bound.h, 1,
            (_, x, y) => new TileGridObject(x, y, CellType.Empty), startPoint);

        for (int x = 0; x < bound.w; x++)
        {
            for (int y = 0; y < bound.h; y++)
            {
                var isWall = x == 0 || y == 0 || x == bound.w - 1 || y == bound.h - 1;
                grid.SetValue(x, y, new TileGridObject(x + bound.x, y + bound.y,
                    isWall ? CellType.Wall : CellType.Ground));
            }
        }
    }
}