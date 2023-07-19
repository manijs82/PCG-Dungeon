using UnityEngine;

public class Simple3DVisual : DungeonVisualizer
{
    public GameObject wall;
    public GameObject ground;
    public GameObject door;
    
    protected override void Visualize(Dungeon dungeon)
    {
        for (int y = 0; y < dungeon.grid.Height; y++)
        {
            for (int x = 0; x < dungeon.grid.Width; x++)
            {
                var tile = (TileGridObject)dungeon.grid.GetValue(x, y);
                var obj = GetObject(tile.Type);
                if(obj == null) continue;
                Instantiate(obj, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }

    private GameObject GetObject(CellType cellType)
    {
        switch (cellType)
        {
            default:
            case CellType.Empty:
                return null;
            case CellType.Wall:
            case CellType.HallwayWall:
                return wall;
            case CellType.Ground:
            case CellType.HallwayGround:    
                return ground;
            case CellType.Door:
                return door;
        }
    }
}