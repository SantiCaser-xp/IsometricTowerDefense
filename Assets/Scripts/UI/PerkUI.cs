using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PerkUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _buyButton;

    private PerkInstance _instance;
    private PerkManager _manager;
    private PerkData _data;

    private void OnEnable()
    {
        if (_instance != null)
            UpdateUI(_instance);
    }

    public void Setup(PerkData data, PerkManager manager)
    {
        _manager = manager;
        _instance = manager.GetInstance(data);

        _icon.sprite = data.Icon;
        _nameText.text = data.Name;
        _descriptionText.text = data.Description;

        _instance.OnChanged += UpdateUI;
        PerkPoints.OnPerksChanged += OnPointsChanged;
        _buyButton.onClick.AddListener(OnBuyClicked);

        UpdateUI(_instance);
    }

    private void UpdateUI(PerkInstance instance)
    {
        _levelText.text = $"Level: {instance.CurrentLevel}/{instance.Data.MaxUpgradeLevel}";
        _priceText.text = $"Price: {instance.CurrentPrice}";

        _buyButton.interactable = instance.CurrentLevel < instance.Data.MaxUpgradeLevel;

        bool canUpgrade = instance.CurrentLevel < instance.Data.MaxUpgradeLevel
                      && _manager.Points.AvailablePerks >= instance.CurrentPrice;

        _buyButton.interactable = canUpgrade;

        _priceText.color = canUpgrade ? Color.white : Color.red;
    }

    private void OnBuyClicked()
    {
        _manager.BuyPerk(_instance.Data);
    }

    private void OnPointsChanged(int value)
    {
        UpdateUI(_instance);
    }

    private void OnDestroy()
    {
        if (_instance != null)
            _instance.OnChanged -= UpdateUI;

        PerkPoints.OnPerksChanged -= OnPointsChanged;
    }
}