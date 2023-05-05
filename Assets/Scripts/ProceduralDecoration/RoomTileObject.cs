using UnityEngine;

public class RoomTileObject : TileGridObject
{
    public TileState tileState;

    public RoomTileObject(int x, int y, CellType type, TileState tileState) : base(x, y, type)
    {
        this.tileState = tileState;
    }

    public Rect GetRect()
    {
        return new Rect(x, y, 1, 1);
    }

    public override bool IsBlocked
    {
        get => false;
    }
}

public enum TileState
{
    Free,
    Ready,
    Occupied
} 