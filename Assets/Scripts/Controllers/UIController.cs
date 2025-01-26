using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textResources;

    private void Awake()
    {
        ResourceManager.ResourceUpdate += UpdateResources;
        UpdateResources(0);
    }

    private void UpdateResources(int amount)
    {
        _textResources.text = $"Total:{amount}";
    }

    private void OnDestroy()
    {
        ResourceManager.ResourceUpdate += UpdateResources;
    }
}