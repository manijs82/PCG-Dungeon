using UnityEngine;

public static class DirectionUtils
{
    public static Vector2Int NorthVec => Vector2Int.up;
    public static Vector2Int EastVec => Vector2Int.right;
    public static Vector2Int WestVec => Vector2Int.left;
    public static Vector2Int SouthVec => Vector2Int.down;
        
    public static Direction GetDirection(this Vector2Int direction)
    {
        if (direction == NorthVec)
            return Direction.North;
        if (direction == EastVec)
            return Direction.East;
        if (direction == WestVec)
            return Direction.West;
        if (direction == SouthVec)
            return Direction.South;

        return default;
    }
        
    public static Vector3 GetVector3(this Direction direction)
    {
        return direction switch
        {
            Direction.North => Vector3.forward,
            Direction.South => Vector3.back,
            Direction.East => Vector3.right,
            Direction.West => Vector3.left,
            _ => Vector3.zero
        };
    }
        
    public static Quaternion GetRotation(this Direction direction)
    {
        return direction switch
        {
            Direction.North => Quaternion.Euler(0, 0, 0),
            Direction.South => Quaternion.Euler(0, 180, 0),
            Direction.East => Quaternion.Euler(0, 90, 0),
            Direction.West => Quaternion.Euler(0, 270, 0),
            _ => Quaternion.identity
        };
    }
        
    public static int GetAngle(this Direction direction)
    {
        return direction switch
        {
            Direction.North => 0,
            Direction.South => 180,
            Direction.East => 270,
            Direction.West => 90,
            _ => 0
        };
    }
}