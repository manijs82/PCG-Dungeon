using UnityEditor;
using UnityEngine;

public class Player2D : Player
{
    [SerializeField] private bool active;
    
    public override bool Active
    {
        get => active;
        set => active = value;
    }

    public override Vector3 GetStartPosition => Dungeon.startRoom.Center;

    public override void Initialize(Dungeon dungeon)
    {
        Dungeon = dungeon;
        transform.position = GetStartPosition;
    }

    public override Vector2 GetCurrentPositionOnDungeon()
    {
        return new Vector2(transform.position.x / Dungeon.bound.w, transform.position.y / Dungeon.bound.h);
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        var gameManager = FindObjectOfType<GameManager>();
        if(gameManager == null) return;
        
        if (active)
        {
            if(gameManager.player != null)
                gameManager.player.Active = false;
            
            gameManager.player = this;
        }
        else
        {
            gameManager.player = null;
        }
    }
    #endif
    
}