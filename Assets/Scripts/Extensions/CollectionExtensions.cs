using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CollectionExtensions
{
    public static T GetRandom<T>(this IEnumerable<T> collect)
    {
        return collect.ElementAt(Random.Range(0, collect.Count()));
    }

    public static List<GraphEdge> GetEdges(this List<GraphNode> nodes)
    {
        List<GraphEdge> edges = new List<GraphEdge>();

        if (nodes.Count > 1)
        {
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                edges.Add(nodes[i].GetEdgeByNode(nodes[i + 1]));
            }
        }

        return edges;
    }

    public static float GetLength(this List<GraphNode> nodes)
    {
        return nodes.GetEdges().Sum(e => e.Weight);
    }
}