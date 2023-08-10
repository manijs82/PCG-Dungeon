using System.Collections.Generic;
using UnityEngine;

public abstract class WorldItem : MonoBehaviour
{
    private static List<WorldItem> spawnedItems = new List<WorldItem>();

    public abstract void OnInteract();

    public static WorldItem Spawn(WorldItem worldItem, Vector2 pos)
    {
        if (worldItem == null) return null;
        var item = Instantiate(worldItem, pos, Quaternion.identity);
        
        spawnedItems.Add(item);
        return item;
    }
    
    public static void Destroy(WorldItem worldItem)
    {
        if (!spawnedItems.Contains(worldItem)) return;

        spawnedItems.Remove(worldItem);
        Object.Destroy(worldItem.gameObject);
    }

    public static void DestroyAll()
    {
        int count = spawnedItems.Count;
        if(count == 0) return;
        for (int i = count - 1; i >= 0; i--)
        {
            Destroy(spawnedItems[i]);
        }
    }
}