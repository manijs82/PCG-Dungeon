using UnityEngine;

public class DungeonVisualizer : MonoBehaviour
{
    protected virtual void Awake()
    {
        Generator.OnDungeonGenerated += Visualize;
    }

    protected virtual void Visualize(Dungeon dungeon)
    {
        
    }
}