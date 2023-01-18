using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Evolution<T> where T : Sample
{
    public const int MaxIterationCount = 100;
    public const int MinGoodScore = 100;
    public const int Population = 20; // gotta be an even number
        
    public List<Sample> samples;

    public Evolution()
    {
        samples = new List<Sample>();

        for (int i = 0; i < Population; i++)
        {
            Dungeon d = new Dungeon();
            d.fitnessValue = Evaluator.EvaluateDungeon(d);
            samples.Add(d);
        }

        Evolute();
    }

    private void Evolute()
    {
        for (int i = 0; i < MaxIterationCount; i++)
        {
            samples = samples.OrderByDescending(d => d.fitnessValue).ToList();
            samples.RemoveRange(Population/2, Population/2);
            for (int j = 0; j < Population/2; j++)
            {
                Dungeon d = new Dungeon((Dungeon) samples[j]);
                d.Mutate();
                d.fitnessValue = Evaluator.EvaluateDungeon(d);
                samples.Add(d);
            }
                
            if(samples[0].fitnessValue >= MinGoodScore) return;
        }
    }
}