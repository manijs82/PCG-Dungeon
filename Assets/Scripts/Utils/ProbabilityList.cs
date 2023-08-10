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
            int weight = Generator.tileRnd.Next(0, GetWeightSum() + 1);

            int currentProbability = list[0].weight;
            for (int i = 0; i < list.Count; i++)
            {
                if(i == list.Count - 1) return list[i].element;
                if(weight <= currentProbability)
                    return list[i].element;
		
                currentProbability += list[i + 1].weight;
            }
	
            return list[0].element;
        }

        public int GetWeightSum()
        {
            int sum = 0;
            foreach (var element in list)
            {
                sum += element.weight;
            }

            return sum;
        }
    }

    [System.Serializable]
    public class ProbableElement<T>
    {
        public T element;
        [Min(0)]
        public int weight = 1;
    }
}