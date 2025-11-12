using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PerkUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _buyButton;
    [SerializeField] PerkData _data;

    private void Awake()
    {
        Setup(_data);
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.OnPerkChanged, UpdateUI);
    }

    private void Start()
    {
        UpdateUI();
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnPerkChanged, UpdateUI);
    }

    public void Setup(PerkData data)
    {
        _icon.sprite = data.Icon;
        _buyButton.onClick.AddListener(OnBuyClicked);
    }

    private void UpdateUI(params object[] parameters)
    {
        bool canUpgrade = ExperienceSystem.Instance.CurrentPerksCount >= _data.BasePrice;

        _buyButton.interactable = canUpgrade;

        _priceText.color = canUpgrade ? Color.white : Color.red;
    }

    private void OnBuyClicked()
    {
        _data.BuyPerk();
        _buyButton.gameObject.SetActive(false);
    }
}