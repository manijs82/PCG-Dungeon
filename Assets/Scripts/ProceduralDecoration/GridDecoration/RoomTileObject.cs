using UnityEngine;
using UnityEngine.Tilemaps;

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

    public override TileChangeData GetTileVisual()
    {
        TileChangeData data = new TileChangeData
        {
            position = new Vector3Int(x, y),
            color = Color.white,
            transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one)
        };

        switch (environmentType)
        {
            case EnvironmentType.Forest:
                data.color = Type == CellType.Wall ? Color.white : ServiceLocator.TileSetProvider.GetGrassColorAt(x, y);
                data.tile = ServiceLocator.TileSetProvider.environmentSet.GetTile(Type);
                break;
            case EnvironmentType.Room:
                data.tile = ServiceLocator.TileSetProvider.riverSet.GetTile(Type);
                break;
            case EnvironmentType.Set:
                data.tile = ServiceLocator.TileSetProvider.sideSet.GetTile(Type);
                break;
            case EnvironmentType.SetTwo:
                data.tile = ServiceLocator.TileSetProvider.sideTwoSet.GetTile(Type);
                break;
        }
        
        return data;
    }
}

public enum TileState
{
    Free,
    Ready,
    Occupied
} 