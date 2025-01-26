using UnityEngine;

[CreateAssetMenu(menuName = "States/Using node State", order = 1)]
class StateUsingNode : State
{
    private float _usingTime;

    protected override void Init()
    {
        if (_fsm is IMiningable miningable)
        {
            _usingTime = _fsm.GetCurrentNode.GetTotalUsingTime(miningable);
        }
        else
        {
            Debug.LogError($"{_fsm.GetTransform.name} haven't IMiningable");
            IsFinished = true;
        }
    }

    protected override void Run()
    {
        _usingTime -= Time.deltaTime;
        if (_usingTime <= 0f)
        {
            _fsm.GetCurrentNode.UseNode(_fsm);
            IsFinished = true;
        }
    }

    public override bool CheckRules(IFSM fsm)
    {
        return fsm.IsFinishedCurrentState();
    }
}