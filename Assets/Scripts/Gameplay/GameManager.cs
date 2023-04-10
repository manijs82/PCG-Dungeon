using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;
    
    private void Start()
    {
        Generator.OnDungeonGenerated += PlacePlayer;        
    }

    private void PlacePlayer(Dungeon dungeon)
    {
        var roomPos = dungeon.rooms[0].Center + new Vector2(dungeon.dungeonParameters.width + 5, 0);
        player.transform.position = roomPos;
        camera.transform.position = new Vector3(roomPos.x, roomPos.y, -10);
    }
}