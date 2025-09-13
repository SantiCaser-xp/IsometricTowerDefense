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

    public void UpdateData(float currentHealth, float maxHealth)
    {
        _sliderLife.value = currentHealth / maxHealth;
        _amountText.SetText($"{currentHealth:F1}/{maxHealth:F1}");
    }

    public void UpdateData(int value) { }
}