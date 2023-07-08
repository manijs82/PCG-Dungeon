using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class DecorationVolume
{
    public EnvironmentType environmentType;
    public ContentType contentType;
    public PlacementSettings currentPlacementSettings;

    private Grid<RoomTileObject> partition;
    private DecorationObjectSource decorationObjectSource;

    public Vector3 Origin => partition.Origin;

    public DecorationVolume()
    {
    }

    public void Init(Room room)
    {
        decorationObjectSource = Resources.Load<DecorationObjectSource>("ObjectSource");
        partition = new Grid<RoomTileObject>(room.bound.w - 2, room.bound.h - 2, 1f, GetDefaultTileObject,
            room.startPoint + Vector2.one);

        currentPlacementSettings = decorationObjectSource.objects[0].placementSettings;
        for (int i = 0; i < 6; i++)
        {
            PlaceTile();
        }
    }

    private void PlaceTile()
    {
        var tile = partition.GetRandomGridObjectWithCondition(HasFreeNeighbors);
        if (tile == null) return;
        Object.Instantiate(decorationObjectSource.objects[0].prefabVariants[0],
            partition.GetWorldPosition(tile.x, tile.y), Quaternion.identity);
        tile.tileState = TileState.Occupied;
        var neighbors = partition.GetNeighbors(tile.x, tile.y, currentPlacementSettings.padding);
        foreach (var neighbor in neighbors) 
            neighbor.tileState = TileState.Ready;
    }

    private RoomTileObject GetDefaultTileObject(Grid<RoomTileObject> grid, int x, int y)
    {
        return new RoomTileObject(x, y, CellType.Ground, TileState.Free);
    }

    private bool HasFreeNeighbors(Grid<RoomTileObject> grid, int x, int y)
    {
        for (int i = x - currentPlacementSettings.padding.x; i <= x + currentPlacementSettings.padding.w; i++)
        {
            for (int j = y - currentPlacementSettings.padding.y; j <= y + currentPlacementSettings.padding.h; j++)
            {
                if(i == x && j == y) continue;
                if (!grid.HasValue(i, j, out RoomTileObject tile)) return false;
                if (tile.tileState != TileState.Free) return false;
            }
        }

        return true;
    }

    #if UNITY_EDITOR
    public void DrawGizmos()
    {
        partition.DrawGizmos(tile =>
        {
            return tile.tileState switch
            {
                TileState.Free => Color.white / 2,
                TileState.Ready => Color.green / 2,
                TileState.Occupied => Color.red / 2,
                _ => throw new ArgumentOutOfRangeException()
            };
        });
    }
    #endif
}