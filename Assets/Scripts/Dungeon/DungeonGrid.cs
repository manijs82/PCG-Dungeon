using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGrid<T> : Grid<T> where T : GridObject
{
    public List<RiverTileObject> riverTiles;
    public List<RoomTileObject> roomTiles;
    public List<HallwayTileObject> hallwayTiles;
    public List<TileGridObject> backgroundTiles;
    
    public DungeonGrid(int width, int height, float cellSize, Func<Grid<T>, int, int, T> createGridObject, Vector3 origin = default, bool shouldDebug = false) : base(width, height, cellSize, createGridObject, origin, shouldDebug)
    {
    }
}