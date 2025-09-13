using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI _levelText;

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

    public void UpdateData(float currentExp, float maxEpx) { }

    public void UpdateData(int value)
    {
        _levelText.SetText($"{value}");
    }
}