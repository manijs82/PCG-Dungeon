﻿using Freya;
using UnityEngine;

[System.Serializable]
public struct Bound
{
    public int x;
    public int y;
    public int w;
    public int h;

    public Vector2 Extents => new Vector2(w / 2f, h / 2f);
    public Vector2 Start => new Vector2(x, y);
    public Vector2 Center => Start + Extents;
    public int XPW => x + w;
    public int YPH => y + h;

    public Bound(int x, int y, int w, int h)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
    }

    public Vector2 ClosestPointInside(Vector2 point)
    {
        Box2D box = new Box2D() { center = Center, extents = Extents };
        Line2D line = new Line2D(point, Center - point);
        var intersect = box.Intersect(line);

        if (Mathfs.Wedge(Vector2.up, point - Center) < 0)
            return intersect.a;
        return intersect.b;
    }

    public static bool Collide(Bound rect1, Bound rect2)
    {
        return rect1.x < rect2.x + rect2.w &&
               rect1.x + rect1.w > rect2.x &&
               rect1.y < rect2.y + rect2.h &&
               rect1.h + rect1.y > rect2.y;
    }

    public static bool Inside(Bound rect1, Bound rect2)
    {
        return rect1.x + rect1.w <= rect2.w &&
               rect1.y + rect1.h <= rect2.h &&
               rect1.x >= rect2.x &&
               rect1.y >= rect2.y;
    }
}