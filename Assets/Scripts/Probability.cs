using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Probability
{
    public static T GetEqualProbability<T>(IEnumerable<T> collection)
    {
        var value = Mathf.RoundToInt(Random.value * (collection.Count() - 1));
        return collection.ElementAt(value);
    }
}