using UnityEngine;

[System.Serializable]
public class NodeData
{
    public string guid;
    public Rect position;
    public bool isEntry;

    public NodeData(string guid, Rect position, bool isEntry = false)
    {
        this.guid = guid;
        this.position = position;
        this.isEntry = isEntry;
    }
}