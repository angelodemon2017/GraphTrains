using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "States/Move State", order = 1)]
class StateMoveToNode : State
{
    private float _moveSpeed;
    private List<GraphNode> _path = new List<GraphNode>();
    private GraphNode _targetNode;
    private GraphEdge _graphEdge;

    protected override void Init()
    {
        if (_fsm is IMovable movable)
        {
            var nodes = CalcingProfitPaths.GetNodes(_fsm);

            _path.AddRange(Pathfinding.GetPath(_fsm.GetCurrentNode,
                _fsm.HaveResources ?
                nodes.Item1 :
                nodes.Item2));
            CheckNode();
            GraphEdge.OnChanged += ChangedEdge;
            GraphNode.OnChanged += ChangedNode;
            if (_fsm is Train train)
            {
                train.OnChanged += UpdateTrain;
            }
        }
        else
        {
            Debug.LogError($"{_fsm.GetTransform.name} haven't IMovable");
            IsFinished = true;
        }
    }

    protected override void Run()
    {
        if (IsFinished)
        {
            return;
        }

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (Vector3.Distance(_fsm.GetTransform.position, _targetNode.transform.position) > 0.1f)
        {
            _fsm.GetTransform.position = Vector3.MoveTowards(_fsm.GetTransform.position, _targetNode.transform.position, _moveSpeed * Time.deltaTime);
        }
        else
        {
            _fsm.SetCurrentNode(_targetNode);
            CheckNode();
        }
    }

    private void ChangedEdge(GraphEdge edge)
    {
        if (_graphEdge == edge)
        {
            UpdateMoveSpeed();
        }
    }

    private void UpdateTrain(Train train)
    {
        UpdateMoveSpeed();
    }

    private void UpdateMoveSpeed()
    {
        _moveSpeed = _graphEdge.GetTotalSpeed(_fsm as IMovable);
    }

    private void ChangedNode(GraphNode node)
    {
        var nodes = CalcingProfitPaths.GetNodes(_fsm);
        var newPath = new List<GraphNode>(Pathfinding.GetPath(_fsm.GetCurrentNode,
                _fsm.HaveResources ? nodes.Item1 : nodes.Item2));
        if (_path.LastOrDefault() != newPath.LastOrDefault())
        {
            _path.Clear();
            if (newPath.Contains(_targetNode))
            {
                while (newPath[0] != _targetNode)
                {
                    newPath.RemoveAt(0);
                }
                _path.AddRange(newPath);
            }
            else
            {
                var pathFromCurrentNode = Pathfinding.GetPath(_fsm.GetCurrentNode, newPath.LastOrDefault());
                _targetNode = _fsm.GetCurrentNode;
                _path.AddRange(pathFromCurrentNode);
            }
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

    public override void ExitState()
    {
        GraphEdge.OnChanged -= ChangedEdge;
    }

    public override bool CheckRules(IFSM fsm)
    {
        return fsm.IsFinishedCurrentState();
    }
}