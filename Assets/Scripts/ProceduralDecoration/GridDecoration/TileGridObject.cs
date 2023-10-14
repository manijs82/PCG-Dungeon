using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGridObject : GridObject
{
    public CellType Type;
    
    protected Grid<GridObject> grid;
    
    public virtual int AnimateSpeed => 50;

    public TileGridObject(int x, int y, Grid<GridObject> grid, CellType type) : base(x, y)
    {
        Type = type;
        this.grid = grid;
    }

    public override bool IsBlocked => Type is CellType.Wall or CellType.Ground;

    public virtual TileChangeData GetTileVisual()
    {
        TileChangeData data = new TileChangeData
        {
            position = new Vector3Int(x, y),
            tile = ServiceLocator.TileSetProvider.hallwaySet.GetTile(Type),
            color = ServiceLocator.TileSetProvider.GetGrassColorAt(x, y),
            transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one)
        };

        foreach (var gridObject in grid.GetNeighbors(x, y, 3))
        {
            if (gridObject is not RoomTileObject roomTile) continue;
            if (roomTile.environmentType != EnvironmentType.Set) continue;
            if (Generator.dungeonRnd.Next(0, 10) > 5 && grid.GetManhattanDistance(this, gridObject) >= 3) continue;

            data.tile = ServiceLocator.TileSetProvider.sideSet.GetTile(Type);
            data.color = Color.white;
            break;
        }

        return data;
    }
}