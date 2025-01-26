using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GraphManager : SingletonMonoBeh<GraphManager>
{
    [SerializeField] private bool _debugLabelsOn;
    [SerializeField] private bool _nodeLabelOn;
    [Range(0, 5)]
    [SerializeField] private float _highNodeLabel;
    [SerializeField] private bool _edgeLabelOn;
    [SerializeField] private Transform _parentNodes;

    private HashSet<GraphNode> _cashNodes = new();
    internal bool NodeLabelOn => _nodeLabelOn && _debugLabelsOn;
    internal bool EdgeLabelOn => _edgeLabelOn && _debugLabelsOn;
    internal float HighNodeLabel => _highNodeLabel;

    internal IEnumerable<T> GetNodes<T>() where T : GraphNode
    {
        return _cashNodes.Where(n => n is T).Select(n => (T)n);
    }

    private void Awake()
    {
        foreach (Transform child in _parentNodes)
        {
            if (child.TryGetComponent(out GraphNode node))
            {
                _cashNodes.Add(node);
            }
        }
    }
}