using Mani.Graph;

public class RoomGrid : GridDecorator
{
    private Graph<Room> roomGraph;

    public RoomGrid(Graph<Room> roomGraph)
    {
        this.roomGraph = roomGraph;
    }
    
    public override void Decorate(DungeonGrid<GridObject> grid)
    {
        foreach (var roomNode in roomGraph.Nodes)
        {
            roomNode.Value.MakeGrid(roomTile => grid.roomTiles.Add(roomTile));
            grid.PlaceGridOnGrid(roomNode.Value.bound.x, roomNode.Value.bound.y, roomNode.Value.grid);
        }
    }
}