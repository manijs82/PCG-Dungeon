using System.Collections;
using UnityEngine;
using Utils;

public class Npc : MonoBehaviour
{
    private Room room;
    private GridObject currentTile;

    private Vector2 GridPositionOffset => new (room.grid.CellSize / 2, room.grid.CellSize / 2);

    public void SetRoom(Room room, GridObject startingTile)
    {
        this.room = room;
        currentTile = startingTile;

        transform.position = startingTile.Point + GridPositionOffset;

        StartCoroutine(MoveToTile(GetRandomTile()));
    }

    private IEnumerator MoveToTile(GridObject tile)
    {
        AStarAlgorithm astar = new AStarAlgorithm(room.grid, currentTile, tile);
        astar.PathFindingSearch();
        astar.path.Remove(currentTile);
        
        foreach (var gridObject in astar.path)
        {
            for (int i = 1; i <= 10; i++)
            {
                yield return new WaitForSeconds(0.1f);
                
                float t = i / 10f;
                transform.position = currentTile.GetPositionTowards(gridObject, t) + GridPositionOffset;
            }

            currentTile = gridObject;
        }
        
        StartCoroutine(MoveToTile(GetRandomTile()));
    }

    private GridObject GetRandomTile()
    {
        return room.grid.GetRandomGridObjectWithCondition(o => ((RoomTileObject)o).Type == CellType.Ground);
    }
}