using UnityEngine;

[CreateAssetMenu(menuName = "States/Thinking State", order = 1)]
class StateThinking : State
{
    protected override void Init()
    {
        IsFinished = true;
    }

    public override bool CheckRules(IFSM fsm)
    {
        return fsm.IsFinishedCurrentState();
    }
}