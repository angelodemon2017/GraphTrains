using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    internal virtual string StateKey => GetType().Name;

    [SerializeField] protected List<State> AvailableStates;

    protected IFSM _fsm;

    public bool IsFinished { get; protected set; }

    public void InitState(IFSM fsm)
    {
        IsFinished = false;
        _fsm = fsm;
        Init();
    }

    protected virtual void Init() { }

    public void RunState()
    {
        Run();
        CheckTransitions();
    }

    protected virtual void Run() { }

    protected void CheckTransitions()
    {
        for (int i = 0; i < AvailableStates.Count; i++)
        {
            if (AvailableStates[i].CheckRules(_fsm))
            {
                _fsm.SetState(AvailableStates[i]);
                break;
            }
        }
    }

    public abstract bool CheckRules(IFSM fsm);

    public virtual void ExitState() { }
}