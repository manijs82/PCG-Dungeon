using UnityEngine;

public static class NpcPlacer
{
    public static void Place(Dungeon dungeon, Npc npcPrefab)
    {
        foreach (var node in dungeon.roomGraph.Nodes)
        {
            if(node.Value.environmentType != EnvironmentType.Room) 
                continue;

            Npc npc = Object.Instantiate(npcPrefab);
            npc.SetRoom(node.Value, node.Value.grid.GetRandomGridObjectWithCondition(o => ((RoomTileObject)o).Type == CellType.Ground));
            break;
        }
    }
}