using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] [HideInInspector] public Player player;
    
    [SerializeField] private Generator generator;
    [SerializeField] private ItemDataBase itemDataBase;
    
    public bool HasKey;
    
    private void Start()
    {
        Generator.OnDungeonGenerated += PlacePlayer;        
    }

    private void PlacePlayer(Dungeon dungeon)
    {
        player.Initialize(dungeon);
    }

    public void GoToNewDungeon()
    {
        if(HasKey)
            generator.GenerateDungeon();
    }

    public ItemDataBase GetItemDB()
    {
        return itemDataBase;
    }
}