public class GraphNodeMine : GraphNode
{
    private int _amountMine = 1;

    protected override string label => $"Mine: {_usingTimeMulti.SimpleFormat()}({base.label})";

    internal override void UseNode(IFSM fsm)
    {
        fsm.Using(_amountMine);
    }

    internal override float GetWeight(IFSM fsm)
    {
        return 1f / (GetTotalUsingTime(fsm as IMiningable) + GetTimeMoveToNode(fsm));
    }
}