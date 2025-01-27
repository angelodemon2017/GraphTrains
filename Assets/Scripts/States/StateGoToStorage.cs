using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Go to storage State", order = 1)]
class StateGoToStorage : StateGoToNode
{
    internal override List<GraphNode> GetPotencialNodes()
    {
        return GraphManager.Instance.GetNodes<GraphNodeStorage>().OfType<GraphNode>().ToList();
    }

    public override bool CheckRules(IFSM fsm)
    {
        return fsm.IsFinishedCurrentState() && fsm.HaveResources;
    }
}