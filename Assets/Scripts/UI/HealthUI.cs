using UnityEngine.UI;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Slider))]
public class HealthUI : MonoBehaviour, IObserver
{
    [SerializeField] private Slider _sliderLife;
    [SerializeField] private TextMeshProUGUI _amountText;

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
        float current = (float)values[0];
        float max = (float)values[1];

        _sliderLife.value = current / max;
        _amountText.SetText($"{current:F1}/{max:F1}");
    }
}