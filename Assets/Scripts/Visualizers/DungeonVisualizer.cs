using UnityEngine;

public class DungeonVisualizer : MonoBehaviour
{
    private void Awake()
    {
        Generator.OnDungeonGenerated += Visualize;
    }

    protected virtual void Visualize(Dungeon dungeon)
    {
        
    }
}