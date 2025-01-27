using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Go to mine State", order = 1)]
class StateGoToMine : StateGoToNode
{
    internal override List<GraphNode> GetPotencialNodes()
    {
        return GraphManager.Instance.GetNodes<GraphNodeMine>().OfType<GraphNode>().ToList();
    }

    public override bool CheckRules(IFSM fsm)
    {
        return fsm.IsFinishedCurrentState() && !fsm.HaveResources;
    }
}