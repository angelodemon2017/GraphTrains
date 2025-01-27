using System.Collections.Generic;
using System.Linq;

public class Pathfinding
{
    private static Dictionary<(GraphNode, GraphNode), (List<GraphNode>,float)> _cashPaths = new();

    private static void CalcPaths(GraphNode node1, GraphNode node2)
    {
        if (!_cashPaths.ContainsKey((node1, node2)))
        {
            var tempPath = FindShortestPath(node1, node2);
            _cashPaths.Add((node1, node2), (FindShortestPath(node1, node2), tempPath.GetLength()));
        }
    }

    internal static float GetWeigh(GraphNode node1, GraphNode node2)
    {
        CalcPaths(node1, node2);

        return _cashPaths[(node1, node2)].Item2;
    }

    internal static List<GraphNode> GetPath(GraphNode node1, GraphNode node2)
    {
        CalcPaths(node1, node2);

        return _cashPaths[(node1, node2)].Item1;
    }

    internal static void UpdatePaths()
    {
        _cashPaths.Clear();
    }

    private static List<GraphNode> FindShortestPath(GraphNode startNode, GraphNode targetNode)
    {
        Dictionary<GraphNode, float> distances = new Dictionary<GraphNode, float>();
        Dictionary<GraphNode, GraphNode> previousNodes = new Dictionary<GraphNode, GraphNode>();
        List<GraphNode> priorityQueue = new List<GraphNode>();

        foreach (var node in FindAllNodes())
        {
            distances[node] = float.MaxValue;
            previousNodes[node] = null;
            priorityQueue.Add(node);
        }
        distances[startNode] = 0;

        while (priorityQueue.Count > 0)
        {
            GraphNode currentNode = ExtractMin(priorityQueue, distances);

            if (currentNode == targetNode)
                break;

            foreach (var edge in currentNode.GetEdges)
            {
                GraphNode neighbor = edge.NodeB == currentNode ? edge.NodeA : edge.NodeB;
                float alt = distances[currentNode] + edge.Weight;

                if (alt < distances[neighbor])
                {
                    distances[neighbor] = alt;
                    previousNodes[neighbor] = currentNode;
                }
            }
        }

        return ReconstructPath(previousNodes, targetNode);
    }

    private static List<GraphNode> FindAllNodes()
    {
        return GraphManager.Instance.GetNodes<GraphNode>().ToList();
    }

    private static GraphNode ExtractMin(List<GraphNode> priorityQueue, Dictionary<GraphNode, float> distances)
    {
        GraphNode minNode = priorityQueue[0];
        foreach (var node in priorityQueue)
        {
            if (distances[node] < distances[minNode])
            {
                minNode = node;
            }
        }
        priorityQueue.Remove(minNode);
        return minNode;
    }

    private static List<GraphNode> ReconstructPath(Dictionary<GraphNode, GraphNode> previousNodes, GraphNode targetNode)
    {
        List<GraphNode> path = new List<GraphNode>();
        GraphNode currentNode = targetNode;

        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = previousNodes[currentNode];
        }

        path.Reverse();
        return path;
    }
}