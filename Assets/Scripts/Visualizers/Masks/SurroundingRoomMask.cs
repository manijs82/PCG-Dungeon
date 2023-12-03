using UnityEngine;

public class SurroundingRoomMask : Mask
{
    private DungeonGrid<GridObject> grid;
    private EnvironmentType roomType;
    
    public SurroundingRoomMask(Dungeon dungeon, EnvironmentType roomType, bool inverted = false) : base(dungeon, inverted)
    {
        grid = dungeon.grid;
        this.roomType = roomType;
    }

    public override bool GetMaskValueAt(int x, int y)
    {
        var gridObj = grid.GetValue(x, y);

        if (gridObj.GetType() != typeof(TileGridObject))
            return false;
        
        foreach (var gridObject in grid.GetNeighbors(x, y, 3))
        {
            if (gridObject is not RoomTileObject roomTile) continue;
            if (roomTile.environmentType != roomType) continue;
            
            if (grid.GetManhattanDistance(gridObj, gridObject) < 3 
                || (Generator.dungeonRnd.Next(0, 10) > 5 && grid.GetManhattanDistance(gridObj, gridObject) == 3))
            {
                return true;
            }
        }
        return false;
    }

    public override Texture2D GetMaskTexture(float resolutionScaling)
    {
        return null;
    }
}