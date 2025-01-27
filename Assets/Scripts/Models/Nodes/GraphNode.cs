using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    [SerializeField] protected float _usingTimeMulti;

    private List<GraphEdge> _edges = new List<GraphEdge>();
    private float _lastUsingTimeMulti;

    internal List<GraphEdge> GetEdges => _edges;
    protected virtual string label => name;

    internal static Action<GraphNode> OnChanged;

    private void OnDrawGizmos()
    {
        if (!GraphManager.Instance.NodeLabelOn)
        {
            return;
        }

        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * GraphManager.Instance.HighNodeLabel - Vector3.up * 0.5f);
        Handles.Label(transform.position + Vector3.up * GraphManager.Instance.HighNodeLabel, $"{label}");
    }

    private void Awake()
    {
        _lastUsingTimeMulti = _usingTimeMulti;
    }

    internal GraphEdge GetEdgeByNode(GraphNode graphNode)
    {
        return _edges.FirstOrDefault(e => e.NodeB == graphNode || e.NodeA == graphNode);
    }

    internal void AddEdge(GraphEdge graphEdge)
    {
        _edges.Add(graphEdge);
    }

    internal virtual void UseNode(IFSM fsm) { }

    private void Update()
    {
        if (_lastUsingTimeMulti != _usingTimeMulti)
        {
            _lastUsingTimeMulti = _usingTimeMulti;
            OnChanged?.Invoke(this);
        }
    }

    internal float GetTotalUsingTime(IMiningable miningable)
    {
        return _usingTimeMulti * miningable.GetMiningSpeed;
    }

    protected float GetTimeMoveToNode(IFSM fsm)
    {
        return Pathfinding.GetWeigh(fsm.GetCurrentNode, this) / (fsm as IMovable).GetBaseMoveSpeed;
    }

    internal virtual float GetWeight(IFSM fsm)
    {
        return 0;
    }
}