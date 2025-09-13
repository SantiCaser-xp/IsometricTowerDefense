using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ExperienceUI : MonoBehaviour, IObserver
{
    [SerializeField] private Slider _sliderExperience;
    [SerializeField] private TextMeshProUGUI _amountText;
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

    public void UpdateData(float currentExp, float maxEpx)
    {
        _sliderExperience.value = currentExp / maxEpx;
        _amountText.SetText($"{currentExp:F2}/{maxEpx:F2}");
    }

    public void UpdateData(int value)
    {
        _levelText.SetText($"{value}");
    }
}