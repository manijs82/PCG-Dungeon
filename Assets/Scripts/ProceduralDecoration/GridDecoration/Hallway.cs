using System.Linq;
using Mani.Graph;

public class Hallway : GridDecorator
{
    private Grid<GridObject> grid;
    private Graph<Room> graph;

    public Hallway(Graph<Room> graph)
    {
        this.graph = graph;
    }
    
    public override void Decorate(Grid<GridObject> grid)
    {
        this.grid = grid;
        
        foreach (var connection in graph.Edges)
        {
            var room1 = connection.Start.Value;
            var room2 = connection.End.Value;
            
            AddDoorsBetweenRooms(room1, room2, out RoomTileObject door1, out RoomTileObject door2);
            
            if (door1 == null || door2 == null) return;
            
            door1.Type = CellType.Door;
            door2.Type = CellType.Door;
            
            AStarAlgorithm astar = new AStarAlgorithm(grid, door1, door2);
            astar.PathFindingSearch();
            foreach (var gridObject in astar.path)
            {
                var tile = (TileGridObject)gridObject;
                tile.Type = CellType.Ground;
                foreach (var neighbor in grid.Get8Neighbors(tile))
                {
                    var n = (TileGridObject)neighbor;
                    if (n.Type == CellType.Empty) n.Type = CellType.Wall;
                }
            }
            
            SetAsDoor(door1);
            SetAsDoor(door2);
        }
    }
    
    private void AddDoorsBetweenRooms(Room room1, Room room2, out RoomTileObject door1, out RoomTileObject door2)
    {
        var door1Pos = room1.bound.ClosestPointInside(room2.Center);
        door1Pos += (room1.Center - door1Pos).normalized / 5;
        var door2Pos = room2.bound.ClosestPointInside(room1.Center);
        door2Pos += (room2.Center - door2Pos).normalized / 5;

        door1 = (RoomTileObject) grid.GetValue(door1Pos);
        door2 = (RoomTileObject) grid.GetValue(door2Pos);
    }
    
    private void SetAsDoor(RoomTileObject tile)
    {
        tile.Type = CellType.Door;

        var walls = grid.Get4Neighbors(tile, true).
            Where(t => t is RoomTileObject { Type: CellType.Wall } tr && tr.room == tile.room).ToList();
        var secondDoor = (TileGridObject) walls[Generator.dungeonRnd.Next(0, walls.Count - 1)];
        secondDoor.Type = CellType.Door;
    }
}