using System;
using UnityEditor;
using UnityEngine;

public class GraphEdge : MonoBehaviour
{
    public GraphNode NodeA;
    public GraphNode NodeB;
    public float Weight;

    private float _lastWeight;

    internal static Action<GraphEdge> OnChanged;

    private void OnValidate()
    {
        if (NodeA == null || NodeB == null)
        {
            transform.position = Vector3.zero;
        }
        else
        {
            transform.position = Vector3.Lerp(NodeA.transform.position, NodeB.transform.position, 0.5f);
            name = $"Edge_{NodeA.name}-{NodeB.name}";
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (GraphManager.Instance.EdgeLabelOn)
        {
            DrawGizmos();
        }
    }

    internal void DrawGizmos()
    {
        GizmoNode(NodeA);
        GizmoNode(NodeB);
        if (NodeA != null && NodeB != null)
        {
            Handles.Label(transform.position + Vector3.up, $"{Weight}");
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 0.5f);
            Gizmos.DrawLine(NodeA.transform.position, NodeB.transform.position);
        }
    }

    private void GizmoNode(GraphNode node)
    {
        if (node != null)
        {
            Gizmos.DrawSphere(node.transform.position, 1f);
        }
    }

    private void Awake()
    {
        NodeA.AddEdge(this);
        NodeB.AddEdge(this);
        _lastWeight = Weight;
    }

    private void Update()
    {
        if (_lastWeight != Weight)
        {
            _lastWeight = Weight;
            OnChanged?.Invoke(this);
//            CalcingProfitPaths.ChangedGraph();
        }
    }

    internal float GetTotalSpeed(IMovable movable)
    {
        return movable.GetBaseMoveSpeed / Weight;
    }
}