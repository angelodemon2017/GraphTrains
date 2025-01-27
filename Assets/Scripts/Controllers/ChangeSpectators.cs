public class ChangeSpectators : SingletonMonoBeh<ChangeSpectators>
{
    //ѕолучаетс€, что это класс костыль и в идеале нужно применить шину событий

    private void Awake()
    {
        GraphEdge.OnChanged += ChangedEdge;
        GraphNode.OnChanged += ChangeNode;
    }

    internal void SpectatorTrain(Train train)
    {
        train.OnChanged += ChangedTrain;
    }

    internal void SpectatorOffTrain(Train train)
    {
        train.OnChanged += ChangedTrain;
    }

    private void ChangedEdge(GraphEdge edge)
    {
        ProfitPaths.ChangedEdge();
    }

    private void ChangeNode(GraphNode node)
    {
        ProfitPaths.UpdateNodes();
    }

    private void ChangedTrain(Train train)
    {
        ProfitPaths.ChangedFSM(train);
    }

    private void OnDestroy()
    {
        GraphEdge.OnChanged -= ChangedEdge;
        GraphNode.OnChanged -= ChangeNode;
    }
}