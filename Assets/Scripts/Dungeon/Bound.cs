using Freya;
using Mani;
using Mani.Geometry;
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
    public Vector2 BottomLeft => new Vector2(x, y);
    public Vector2 BottomRight => new Vector2(XPW, y);
    public Vector2 TopLeft => new Vector2(x, YPH);
    public Vector2 TopRight => new Vector2(XPW, YPH);
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
        Line2D line2D = new Line2D(Center, Center - point);
        var intersect = box.Intersect(line2D);

        if (Vector2.Dot(Center - point, Center - intersect.a) > 0)
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
    
    public static bool Collide(Circle circle, Bound rect)
    {
        float testX = circle.Center.x;
        float testY = circle.Center.y;

        if (circle.Center.x < rect.x) testX = rect.x;
        else if (circle.Center.x > rect.XPW) testX = rect.XPW;
        if (circle.Center.y < rect.y) testY = rect.y;
        else if (circle.Center.y > rect.YPH) testY = rect.YPH;

        float distance = Point.Distance(circle.Center, new Point(testX, testY));

        return distance <= circle.Radius;
    }

    public static bool Inside(Bound rect1, Bound rect2)
    {
        return rect1.x + rect1.w <= rect2.w &&
               rect1.y + rect1.h <= rect2.h &&
               rect1.x >= rect2.x &&
               rect1.y >= rect2.y;
    }
    
    public static bool Inside(Vector2 point, Bound bound)
    {
        return point.x > bound.x && point.x < bound.XPW &&
               point.y > bound.y && point.y < bound.YPH;
    }
}