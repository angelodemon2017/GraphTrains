using System;

public class ResourceManager : SingletonMonoBeh<ResourceManager>
{
    public static Action<int> ResourceUpdate;

    private int _resources;

    public void AddResource(int profit)
    {
        _resources += profit;
        ResourceUpdate?.Invoke(_resources);
    }
}