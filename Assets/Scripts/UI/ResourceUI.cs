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

    public void UpdateData(params object[] values)
    {
        int count = (int)values[0];
        _countText.SetText($"{count}");
    }
}