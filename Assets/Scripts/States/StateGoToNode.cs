using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Go to node State", order = 1)]
abstract class StateGoToNode : State
{
    [SerializeField] private State _stateThinking;
    private float _moveSpeed;
    private List<GraphNode> _path = new List<GraphNode>();
    private GraphNode _targetNode;
    private GraphEdge _graphEdge;

    internal List<GraphNode> CurrentPath => _path;

    protected override void Init()
    {
        var tempNode = ProfitPaths.GetPreferNode(_fsm, this);
        _path.AddRange(Pathfinding.GetPath(_fsm.GetCurrentNode, tempNode));

        CheckNode();

        GraphEdge.OnChanged += ChangedEdge;
        GraphNode.OnChanged += ChangedNode;
        if (_fsm is Train train)
        {
            train.OnChanged += UpdateTrain;
        }
    }

    protected override void Run()
    {
        if (IsFinished)
        {
            return;
        }

        if ((_fsm as IMovable).Move(_targetNode.transform.position, _moveSpeed))
        {
            _fsm.SetCurrentNode(_targetNode);
            CheckNode();
        }
    }

    private void CheckNode()
    {
        _path.RemoveAt(0);
        if (_path.Count > 0)
        {
            _targetNode = _path[0];
            _graphEdge = _fsm.GetCurrentNode.GetEdgeByNode(_targetNode);
            UpdateMoveSpeed();
        }
        else
        {
            IsFinished = true;
        }
    }

    private void UpdateMoveSpeed()
    {
        _moveSpeed = _graphEdge.GetTotalSpeed(_fsm as IMovable);
    }

    private void ChangedEdge(GraphEdge edge)
    {
        if (_graphEdge == edge)
        {
            UpdateMoveSpeed();
        }
        else
        {
            _fsm.SetState(_stateThinking);
        }
    }

    private void UpdateTrain(Train train)
    {
        UpdateMoveSpeed();
    }

    private void ChangedNode(GraphNode node)
    {
        _fsm.SetState(_stateThinking);
    }

    public override void ExitState()
    {
        GraphEdge.OnChanged -= ChangedEdge;
        GraphNode.OnChanged -= ChangedNode;
        if (_fsm is Train train)
        {
            train.OnChanged -= UpdateTrain;
        }
    }
}