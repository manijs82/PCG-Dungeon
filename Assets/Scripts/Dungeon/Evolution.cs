using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Evolution<T> where T : Sample
{
    public const int MaxIterationCount = 200;
    public const int Population = 2; // gotta be an even number
        
    public List<Sample> samples;

    public Evolution(SampleParameters parameters)
    {
        samples = new List<Sample>();

        for (int i = 0; i < Population; i++)
        {
            T sample = (T)Activator.CreateInstance(typeof(T), parameters);
            sample.fitnessValue = sample.Evaluate();
            samples.Add(sample);
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
                T sample = (T)Activator.CreateInstance(typeof(T), (T) samples[j]);
                sample.Mutate();
                sample.fitnessValue = sample.Evaluate();
                samples.Add(sample);
            }
                
            //if(samples[0].fitnessValue >= samples[0].optimalFitnessValue)
                //return;
        }
    }
}