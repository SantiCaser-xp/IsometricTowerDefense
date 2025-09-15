using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI _countText;

    private void Awake()
    {
        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        obs.Subscribe(this);
    }

    public void UpdateData(float currentValue, float maxValue)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateData(int value)
    {
        _countText.SetText($"{value}");
    }
}