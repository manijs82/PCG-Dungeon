using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform cam;
    
    private void Start()
    {
        Generator.OnDungeonGenerated += PlacePlayer;        
    }

    private void PlacePlayer(Dungeon dungeon)
    {
        var roomPos = dungeon.roomGraph.Nodes[0].Value.Center;
        player.transform.position = roomPos;
    }
}