using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public class ProbabilityList<T>
    {
        public List<ProbableElement<T>> list;

        public T GetWeightedRandomElement()
        {
            float weight = Generator.tileRnd.Next(0, 10) / 10f;

            float currentProbability = list[0].probability;
            for (int i = 0; i < list.Count; i++)
            {
                if(i == list.Count - 1) return list[i].element;
                if(weight <= currentProbability)
                    return list[i].element;
		
                currentProbability += list[i + 1].probability;
            }
	
            return list[0].element;
        }
    }

    [System.Serializable]
    public class ProbableElement<T>
    {
        public T element;
        [Range(0, 1)]
        public float probability;
    }
}