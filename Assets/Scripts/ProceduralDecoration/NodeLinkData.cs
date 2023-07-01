[System.Serializable]
public class NodeLinkData
{
    public string startNodeGuid;
    public string endNodeGuid;

    public NodeLinkData(string startNodeGuid, string endNodeGuid)
    {
        this.startNodeGuid = startNodeGuid;
        this.endNodeGuid = endNodeGuid;
    }
}