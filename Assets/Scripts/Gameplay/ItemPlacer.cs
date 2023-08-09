using Interaction;
using UnityEngine;

public class ItemPlacer : DungeonVisualizer
{
    [SerializeField] private Key key;
    [SerializeField] private Portal portal;
    
    protected override void Visualize(Dungeon dungeon)
    {
        var keyRoom = dungeon.roomGraph.Nodes[Generator.dungeonRnd.Next(0, dungeon.roomGraph.NodeCount - 1)].Value;

        Instantiate(key, keyRoom.Center, Quaternion.identity);
        Instantiate(portal, dungeon.endRoom.Center, Quaternion.identity);
    }
}