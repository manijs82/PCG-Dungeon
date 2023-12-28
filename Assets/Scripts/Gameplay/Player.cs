using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public virtual bool Active { get; set; }
    public virtual Dungeon Dungeon { get; set; }
    public virtual Vector3 GetStartPosition { get; }
    
    public abstract void Initialize(Dungeon dungeon);
    public abstract Vector2 GetCurrentPositionOnDungeon();
}