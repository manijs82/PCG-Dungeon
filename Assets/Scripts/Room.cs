using System;
using UnityEngine;

public struct Room
{
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
}