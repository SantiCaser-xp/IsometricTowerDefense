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

    public void UpdateData(params object[] values)
    {
        float currentExp = (float)values[0];
        float maxExp = (float)values[1];
        int level = (int)values[2];

        _sliderExperience.value = currentExp / maxExp;
        _amountText.SetText($"{currentExp:F2}/{maxExp:F2}");
        _levelText.SetText($"{level}");
    }
}