using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalStamineUI : MonoBehaviour, IObserver
{
    [SerializeField] private Slider _stamineSlider;
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
        _stamineSlider.value = currentValue / maxValue;
        _countText.SetText($"{currentValue:F0}/{maxValue:F0}");
    }

    public void UpdateData(int value) { }
}