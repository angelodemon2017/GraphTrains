using System;
using UnityEngine;

public class Train : MonoBehaviour, IFSM, IMiningable, IMovable
{
    [SerializeField] private float _speedMove;
    [SerializeField] private float _speedMine;
    [SerializeField] private State _startingState;

    private State _currentState;
    private GraphNode _currentNode;
    private float _lastSpeedMove;
    private float _lastSpeedMine;
    private int _resource;

    public float GetMiningSpeed => _speedMine;
    public float GetMoveSpeed => _speedMove;
    public GraphNode GetCurrentNode => _currentNode;

    public Transform GetTransform => transform;
    public bool HaveResources => _resource > 0;

    internal Action<Train> OnChanged;

    private void Awake()
    {
        _lastSpeedMove = _speedMove;
        _lastSpeedMine = _speedMine;
        ChangeSpectators.Instance.SpectatorTrain(this);
    }

    private void Start()
    {
        _currentNode = GraphManager.Instance.GetNodes<GraphNode>().GetRandom();
        transform.position = _currentNode.transform.position;
        SetState(_startingState);
    }

    private void Update()
    {
        _currentState?.RunState();

        if (_lastSpeedMove != _speedMove)
        {
            _lastSpeedMove = _speedMove;
            OnChanged?.Invoke(this);
        }
        if (_lastSpeedMine != _speedMine)
        {
            _lastSpeedMine = _speedMine;
            OnChanged?.Invoke(this);
        }
    }

    public void SetState(State state)
    {
        if (_currentState != null && _currentState.StateKey == state.StateKey)
        {
            return;
        }

        _currentState?.ExitState();
        if (_currentState != null)
        {
            Destroy(_currentState);
        }

        _currentState = Instantiate(state);
        _currentState.InitState(this);
    }

    public bool IsFinishedCurrentState()
    {
        return _currentState.IsFinished;
    }

    public void SetCurrentNode(GraphNode node)
    {
        _currentNode = node;
    }

    public void Mining(int amount)
    {
        _resource += amount;
    }

    public int GetResources()
    {
        var result = _resource;
        _resource = 0;
        return result;
    }

    private void OnDestroy()
    {
        if (ChangeSpectators.Instance != null)
        {
            ChangeSpectators.Instance.SpectatorOffTrain(this);
        }
    }
}