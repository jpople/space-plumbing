using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class AwaUtils {
    // Start is called before the first frame update
    public static float FuzzyRandom(float min = 0f, float max = 0f) {
        // generates a random number between min and max following a normal distribution, more or less
        // taken from here: https://answers.unity.com/questions/421968/normal-distribution-random.html
        float u, v, S;
        do {
            u = 2f * Random.value - 1.0f;
            v = 2f * Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1f);

        float std = u * Mathf.Sqrt(-2f * Mathf.Log(S) / S);

        float mean = (min + max / 2f);
        float sigma = (max - mean) / 3f;
        return Mathf.Clamp(std * sigma + mean, min, max);
    }

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source) {
        // syntax to call this:
        // foreach (var (item, index) in collection.WithIndex());
        return source.Select((item, index) => (item, index));
    }
}
