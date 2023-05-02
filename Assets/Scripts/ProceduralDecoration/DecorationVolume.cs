using UnityEngine;

public class DecorationVolume
{
    public EnvironmentType environmentType;
    public ContentType contentType;
    public DecorationObjectSource decorationObjectSource;

    private Grid<RoomTileObject> partition;

    public DecorationVolume(Room room)
    {
        decorationObjectSource = Resources.Load<DecorationObjectSource>("ObjectSource");
        partition = new Grid<RoomTileObject>(room.bound.w - 2, room.bound.h - 2, 1f, GetDefaultTileObject,
            room.startPoint + Vector2.one);

        PlaceTile();
        PlaceTile();
        PlaceTile();
    }

    private void PlaceTile()
    {
        var tile = partition.GetRandomGridObjectWithCondition(HasFreeNeighbors);
        Object.Instantiate(decorationObjectSource.objects[0].prefabVariants[0],
            partition.GetWorldPosition(tile.x, tile.y), Quaternion.identity);
        tile.tileState = TileState.Occupied;
        foreach (var neighbor in partition.Get8Neighbors(tile))
            neighbor.tileState = TileState.Ready;
    }

    public DecorationVolume(DecorationVolume decorationVolume, Grid<RoomTileObject> partition)
    {
    }

    private RoomTileObject GetDefaultTileObject(Grid<RoomTileObject> grid, int x, int y)
    {
        return new RoomTileObject(x, y, CellType.Ground, TileState.Free);
    }

    private bool HasFreeNeighbors(Grid<RoomTileObject> grid, int x, int y)
    {
        foreach (var neighbor in grid.Get8Neighbors(x, y))
        {
            if (neighbor.tileState != TileState.Free)
                return false;
        }

        return true;
    }
}