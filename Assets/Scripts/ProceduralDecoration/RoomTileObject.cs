using UnityEngine;

public class RoomTileObject : TileGridObject
{
    public TileState tileState;
    public EnvironmentType environmentType;
    public Room room;

    public RoomTileObject(int x, int y, CellType type, Room room, TileState tileState = TileState.Free, EnvironmentType environmentType = EnvironmentType.Room) : base(x, y, type)
    {
        this.tileState = tileState;
        this.environmentType = environmentType;
        this.room = room;
    }

    public Rect GetRect()
    {
        return new Rect(x, y, 1, 1);
    }
}

public enum TileState
{
    Free,
    Ready,
    Occupied
} 