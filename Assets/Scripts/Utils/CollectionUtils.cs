using System.Collections.Generic;

namespace Utils
{
    public static class CollectionUtils
    {
        public static void RemoveValues<T>(this IList<T> list, IEnumerable<T> valuesToRemove)
        {
            foreach (var value in valuesToRemove)
            {
                if (list.Contains(value)) 
                    list.Remove(value);
            }
        }
    }
}