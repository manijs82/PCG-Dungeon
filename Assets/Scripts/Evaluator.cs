using UnityEngine;

public static class Evaluator
{
    public static float EvaluateDungeon(Dungeon dungeon)
    {
        float finalScore = 0;

        foreach (var room in dungeon.rooms)
            finalScore += EvaluateRoom(room);

        finalScore += EvaluateCollision(dungeon);

        return finalScore;
    }

    private static float EvaluateCollision(Dungeon dungeon)
    {
        float score = 0;

        for (int i = 0; i < dungeon.rooms.Count; i++)
        {
            if (!Bound.Inside(dungeon.rooms[i].Bound, new Bound(0, 0, 50, 50)))
                score -= 1000;

            if (i == dungeon.rooms.Count - 1) break;
            for (int j = i + 1; j < dungeon.rooms.Count; j++)
            {
                if (Bound.Collide(dungeon.rooms[i].Bound, dungeon.rooms[j].Bound))
                    score -= 100;
            }
        }

        return score;
    }

    private static float EvaluateRoom(Room room)
    {
        Vector2 center = new Vector2(25f, 25f);

        float score = 25 - (room.Center - center).magnitude;
        return score;
    }
}