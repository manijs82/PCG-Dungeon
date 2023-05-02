public class RoomTileObject : TileGridObject
{
    public TileState tileState;

    public RoomTileObject(int x, int y, CellType type, TileState tileState) : base(x, y, type)
    {
        this.tileState = tileState;
    }
}

public enum TileState
{
    Free,
    Ready,
    Occupied
} 