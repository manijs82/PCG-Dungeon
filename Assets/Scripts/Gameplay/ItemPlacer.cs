using UnityEngine;

public class ItemPlacer : DungeonVisualizer
{
    protected override void Visualize(Dungeon dungeon)
    {
        WorldItem.DestroyAll();
        
        var itemDB = GameManager.Instance.GetItemDB();
        if (itemDB == null) return;
        
        var keyRoom = dungeon.roomGraph.Nodes[Generator.dungeonRnd.Next(0, dungeon.roomGraph.NodeCount - 1)].Value;
        var key = itemDB.GetItem<Key>();
        var portal = itemDB.GetItem<Portal>();

        WorldItem.Spawn(key, keyRoom.Center);
        WorldItem.Spawn(portal, dungeon.endRoom.Center);
    }
}