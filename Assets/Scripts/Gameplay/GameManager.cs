using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform cam;
    [SerializeField] private Generator generator;
    
    public bool HasKey;
    
    private void Start()
    {
        Generator.OnDungeonGenerated += PlacePlayer;        
    }

    private void PlacePlayer(Dungeon dungeon)
    {
        var roomPos = dungeon.startRoom.Center;
        player.transform.position = roomPos;
    }

    public void GoToNewDungeon()
    {
        if(HasKey)
            generator.GenerateDungeon();
    }

    public PlayerController GetPlayer()
    {
        return player.GetComponent<PlayerController>();
    }
}