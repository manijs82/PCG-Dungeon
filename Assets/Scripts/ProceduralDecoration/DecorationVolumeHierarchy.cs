using System.Collections.Generic;
using ManisDataStructures.Hierarchy;
using UnityEngine;

[CreateAssetMenu(fileName = "DecorationVolume", menuName = "Decoration Volume", order = 0)]
public class DecorationVolumeHierarchy : ScriptableObject
{
    public Hierarchy<DecorationVolume> volumeHierarchy;
    [HideInInspector] public GraphViewData graphViewData;
}

[System.Serializable]
public class GraphViewData
{
    public List<NodeData> nodes;
    public List<NodeLinkData> links;

    public GraphViewData()
    {
        nodes = new List<NodeData>();
        links = new List<NodeLinkData>();
    }
}