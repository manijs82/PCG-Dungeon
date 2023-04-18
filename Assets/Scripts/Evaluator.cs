using System.Collections.Generic;
using Freya;
using UnityEngine;

public static class Evaluator
{
    public static float EvaluateDungeonBasedOnLineBetweenRooms(Dungeon dungeon)
    {
        float finalScore = 0;

        List<LineSegment2D> lines = new List<LineSegment2D>();
        for (int i = 0; i < dungeon.rooms.Count - 1; i++)
        {
            var startRoom = dungeon.rooms[i];
            var endRoom = dungeon.rooms[i+1];

            var endCenter = endRoom.Center + (startRoom.Center - endRoom.Center).normalized / 10f;
            var line = new LineSegment2D(startRoom.Center, endCenter);
            lines.Add(line);
            if (lines.Count <= 1) continue;

            bool didIntersect = false;
            for (int j = lines.Count - 2; j >= 0; j--)
            {
                if (line.Intersects(lines[j]))
                {
                    didIntersect = true;
                    finalScore--;
                }
            }

            if (!didIntersect)
                finalScore++;
        }

        finalScore += EvaluateCollision(dungeon);

        return finalScore;
    }
    
    public static float EvaluateDungeonBasedOnDistanceFromCenter(Dungeon dungeon)
    {
        float finalScore = 0;

        foreach (var room in dungeon.rooms)
            finalScore += EvaluateRoom(room, dungeon);

        finalScore += EvaluateCollision(dungeon);

        return finalScore;
    }

    private static float EvaluateRoom(Room room, Dungeon dungeon)
    {
        Vector2 center = new Vector2(dungeon.dungeonParameters.width/2f, dungeon.dungeonParameters.height/2f);

        float score = center.magnitude - (room.Center - center).magnitude;
        return score;
    }

    private static float EvaluateCollision(Dungeon dungeon)
    {
        float score = 0;

        for (int i = 0; i < dungeon.rooms.Count; i++)
        {
            if (!Bound.Inside(dungeon.rooms[i].bound, new Bound(0, 0, dungeon.dungeonParameters.width, dungeon.dungeonParameters.height)))
                score -= 10000;

            if (i == dungeon.rooms.Count - 1) break;
            for (int j = i + 1; j < dungeon.rooms.Count; j++)
            {
                if (Bound.Collide(dungeon.rooms[i].bound, dungeon.rooms[j].bound))
                    score -= 5000;
            }
        }

        return score;
    }
}