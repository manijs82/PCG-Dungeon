using UnityEngine.Serialization;

[System.Serializable]
public class DungeonSaveData
{
    public DungeonParameters dungeonParameters;
    public Bound[] roomBounds;
    public EnvironmentType[] roomsEnvironmentTypes;
    public int[] connectionStartIndices;
    public int[] connectionEndIndices;
}