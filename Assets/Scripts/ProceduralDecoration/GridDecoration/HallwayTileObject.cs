using UnityEngine;
using UnityEngine.Tilemaps;

public class HallwayTileObject : TileGridObject
{
    public bool isOverRiver;

    public override int AnimateSpeed => 8;

    public HallwayTileObject(int x, int y, Grid<GridObject> grid, CellType type) : base(x, y, grid, type)
    {
    }

    public override TileChangeData GetTileVisual()
    {
        TileChangeData data = new TileChangeData
        {
            position = new Vector3Int(x, y),
            color = Color.white,
            transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one)
        };

        data.tile = !isOverRiver
            ? ServiceLocator.TileSetProvider.hallwaySet.GetTile(Type)
            : ServiceLocator.TileSetProvider.riverSet.GetTile(Type);

        return data;
    }
}