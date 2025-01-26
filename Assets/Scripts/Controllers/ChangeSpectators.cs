public class ChangeSpectators : SingletonMonoBeh<ChangeSpectators>
{
    //����������, ��� ��� ����� ������� � � ������ ����� ��������� ���� �������

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
        CalcingProfitPaths.ChangedEdge();
    }

    private void ChangeNode(GraphNode node)
    {
        CalcingProfitPaths.UpdateNodes();
    }

    private void ChangedTrain(Train train)
    {
        CalcingProfitPaths.ChangedFSM(train);
    }

    private void OnDestroy()
    {
        GraphEdge.OnChanged -= ChangedEdge;
        GraphNode.OnChanged -= ChangeNode;
    }
}