using System.Collections;
using UnityEngine;
using Utils;

public class Npc : MonoBehaviour
{
    private const int AnimationSteps = 20;
    
    [Tooltip("Tile per second")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector2 waitTimeRange = new (1f, 3f);
    
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
        astar.PathFindingSearch(true);
        astar.path.Remove(currentTile);
        astar.path.Reverse();
        
        print(astar.path.Count);
        foreach (var gridObject in astar.path)
        {
            for (int i = 1; i <= AnimationSteps; i++)
            {
                yield return new WaitForSeconds(speed * (1f / AnimationSteps));
                
                float t = (float)i / AnimationSteps;
                transform.position = currentTile.GetPositionTowards(gridObject, t) + GridPositionOffset;
            }

            currentTile = gridObject;
        }

        yield return new WaitForSeconds(Random.Range(waitTimeRange.x, waitTimeRange.y));
        StartCoroutine(MoveToTile(GetRandomTile()));
    }

    private GridObject GetRandomTile()
    {
        return room.grid.GetRandomGridObjectWithCondition(o => ((RoomTileObject)o).Type == CellType.Ground);
    }
}