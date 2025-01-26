using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CollectionExtensions
{
    public static T GetRandom<T>(this IEnumerable<T> collect)
    {
        return collect.ElementAt(Random.Range(0, collect.Count()));
    }

    public static float GetLength(this List<GraphNode> nodes)
    {
        if (nodes.Count < 2)
        {
            return 0;
        }

        float result = 0f;

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            result += nodes[i].GetEdgeByNode(nodes[i + 1]).Weight;
        }

        return result;
    }
}