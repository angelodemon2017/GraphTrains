using System.Collections.Generic;

class ProfitPaths
{
    /// <summary>
    /// IFSM - entity
    /// string - keyState
    /// GraphNode - currentNode
    /// </summary>
    private static Dictionary<IFSM, Dictionary<(string, GraphNode), GraphNode>> _cashProfitNode = new();

    internal static GraphNode GetPreferNode(IFSM unit, State state)
    {
        if (!_cashProfitNode.ContainsKey(unit))
        {
            _cashProfitNode.Add(unit, new());
        }

        if (!_cashProfitNode[unit].ContainsKey((state.StateKey, unit.GetCurrentNode)))
        {
            var nodes = state.GetPotencialNodes();
            GraphNode preferNode = nodes[0];
            float profit = nodes[0].GetWeight(unit);

            foreach (var node in nodes)
            {
                if (node.GetWeight(unit) > profit)
                {
                    profit = node.GetWeight(unit);
                    preferNode = node;
                }
            }

            _cashProfitNode[unit].Add((state.StateKey, unit.GetCurrentNode), preferNode);
        }

        return _cashProfitNode[unit][(unit.KeyCurrentState, unit.GetCurrentNode)];
    }

    internal static void ChangedEdge()
    {
        Pathfinding.UpdatePaths();
        _cashProfitNode.Clear();
    }

    internal static void UpdateNodes()
    {
        _cashProfitNode.Clear();
    }

    internal static void ChangedFSM(IFSM unit)
    {
        _cashProfitNode.Remove(unit);
    }
}