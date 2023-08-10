using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB", menuName = "Dungeon/ItemDataBase", order = 0)]
public class ItemDataBase : ScriptableObject
{
    public WorldItem[] items;

    public T GetItem<T>() where T : WorldItem
    {
        foreach (var item in items)
        {
            if (item is T i)
                return i;
        }

        return null;
    }
}