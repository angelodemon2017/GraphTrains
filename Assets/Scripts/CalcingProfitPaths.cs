using System.Collections.Generic;

class CalcingProfitPaths
{
    private static Dictionary<(GraphNodeStorage, GraphNodeMine), float> _cashWeights = new();

    private static Dictionary<(GraphNodeStorage, GraphNodeMine), float> _getWeights
    {
        get
        {
            if (_cashWeights.Count == 0)
            {
                var storages = GraphManager.Instance.GetNodes<GraphNodeStorage>();
                var mines = GraphManager.Instance.GetNodes<GraphNodeMine>();

                foreach (var storage in storages)
                    foreach (var mine in mines)
                    {
                        float length = Pathfinding.GetPath(storage, mine).GetLength();
                        _cashWeights.Add((storage, mine), length);
                    }
            }

            return _cashWeights;
        }
    }

    private static Dictionary<IFSM, (GraphNodeStorage, GraphNodeMine)> _cashPreferPares = new();
    private static (GraphNodeStorage, GraphNodeMine) GetBetterPare(IFSM unit)
    {
        if (!_cashPreferPares.ContainsKey(unit))
        {
            float profit = 0f;
            (GraphNodeStorage, GraphNodeMine) selectPare = new();
            foreach (var pare in _getWeights)
            {
                var tempProfit = pare.Key.Item1.Multiplicator / (pare.Value / ((IMovable)unit).GetMoveSpeed * 2 + pare.Key.Item2.GetTotalUsingTime(unit as IMiningable));
                if (tempProfit > profit)
                {
                    profit = tempProfit;
                    selectPare = pare.Key;
                }
            }
            _cashPreferPares.Add(unit, selectPare);
        }

        return _cashPreferPares[unit];
    }

    internal static void ChangedEdge()
    {
        Pathfinding.UpdatePaths();
        _cashWeights.Clear();
    }

    internal static void UpdateNodes()
    {
        _cashPreferPares.Clear();
    }

    internal static void ChangedFSM(IFSM unit)
    {
        _cashPreferPares.Remove(unit);
    }

    internal static (GraphNodeStorage, GraphNodeMine) GetNodes(IFSM unit)
    {
        return GetBetterPare(unit);
    }
}