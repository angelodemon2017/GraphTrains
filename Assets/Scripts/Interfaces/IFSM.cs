using System;
using UnityEngine;

public interface IFSM
{
    bool HaveResources { get; }

    void Mining(int amount);

    int GetResources();

    GraphNode GetCurrentNode { get; }

    bool IsFinishedCurrentState();

    void SetState(State state);

    void SetCurrentNode(GraphNode node);

    Transform GetTransform { get; }
}