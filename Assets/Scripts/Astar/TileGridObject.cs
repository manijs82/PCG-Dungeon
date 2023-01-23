public class TileGridObject : GridObject
{
    public CellType Type;

    public TileGridObject(int x, int y, CellType type) : base(x, y)
    {
        Type = type;
    }

    public override bool IsBlocked => Type == CellType.Wall;
}