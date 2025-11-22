using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalStamineUI : MonoBehaviour, IObserver
{
    [SerializeField] private Slider _stamineSlider;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _timerText;

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
        TimeSpan time = (TimeSpan)values[2];

        if (current >= max)
        {
            _timerText.SetText("Full");
            return;
        }

        _stamineSlider.value = current / max;
        _timerText.SetText($"{time.Hours.ToString("00")}:{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}");
        _countText.SetText($"{current:F0}/{max:F0}");
    }
}