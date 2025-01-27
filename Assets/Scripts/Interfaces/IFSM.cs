using UnityEngine;

public interface IFSM
{
    bool HaveResources { get; }

    void Using(int amount);

    int GetResources();

    GraphNode GetCurrentNode { get; }

    bool IsFinishedCurrentState();

    void SetState(State state);

    void SetCurrentNode(GraphNode node);

    Transform GetTransform { get; }
}