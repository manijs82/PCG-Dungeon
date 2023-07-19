public abstract class Sample
{
    public float optimalFitnessValue;
    public float fitnessValue;
    
    public virtual void Mutate() { }
    public virtual float Evaluate() { return 0; }
        
}

public class SampleParameters
{
    
}