using UnityEngine;

public class GraphNodeStorage : GraphNode
{
    [SerializeField] private int _multiplicator = 1;

    private int _lastMultiplicator;

    protected override string label => $"Storage: {_multiplicator}õ({base.label})";
    internal int Multiplicator => _multiplicator;

    internal override void UseNode(IFSM fsm)
    {
        var result = fsm.GetResources();
        ResourceManager.Instance.AddResource(_multiplicator * result);
    }

    private void Update()
    {
        if (_lastMultiplicator != _multiplicator)
        {
            _lastMultiplicator = _multiplicator;
            OnChanged?.Invoke(this);
        }
    }
}