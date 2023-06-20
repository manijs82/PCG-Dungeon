using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class DecorationVolume
{
    public EnvironmentType environmentType;
    public ContentType contentType;
    public DecorationObjectSource decorationObjectSource;

    private Grid<RoomTileObject> partition;
    private PlacementSettings currentPlacementSettings;

    public Vector3 Origin => partition.Origin;

    public DecorationVolume(Room room)
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

    public DecorationVolume(DecorationVolume decorationVolume, Grid<RoomTileObject> partition)
    {
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
        Handles.color = Color.white;
        Color color = Color.black;
        
        foreach (var obj in partition.GridObjects)
        {
            switch (obj.tileState)
            {
                case TileState.Free:
                    color = Color.white / 2;
                    break;
                case TileState.Ready:
                    color = Color.green / 2;
                    break;
                case TileState.Occupied:
                    color = Color.red / 2;
                    break;
            }    
            
            Handles.DrawSolidRectangleWithOutline(new Rect(Origin.x + obj.x, Origin.y + obj.y, 1, 1), color, color);
        }
        
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