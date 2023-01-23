using UnityEngine;

public class GridObject
{
    public int x, y;
    
    protected bool isBlocked;

    public Vector2 Point => new (x, y);
    public Vector2Int PointInt => new (x, y);
    public int Cost { get; set; } = 1;
    public virtual bool IsBlocked => isBlocked;

    public GridObject(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}