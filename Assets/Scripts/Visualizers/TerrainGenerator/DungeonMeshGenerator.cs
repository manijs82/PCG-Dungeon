public abstract class DungeonMeshGenerator
{
    protected Dungeon dungeon;
    protected NoiseMap heightMap;
    
    public DungeonMeshGenerator(Dungeon dungeon, NoiseMap heightMap)
    {
        this.dungeon = dungeon;
        this.heightMap = heightMap;
    }

    public abstract void Generate();
}