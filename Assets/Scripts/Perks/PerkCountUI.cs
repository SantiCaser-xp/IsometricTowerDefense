using TMPro;
using UnityEngine;

public class PerkCountUI : MonoBehaviour
{
    TextMeshProUGUI _countText;

    private void Awake()
    {
        _countText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.OnPerkChanged, ChangeCountText);

    }

    private void Start()
    {
        ChangeCountText();
    }

    private void OnDisable()
    {
        EventManager.Subscribe(EventType.OnPerkChanged, ChangeCountText);
    }

    private void ChangeCountText(params object[] parameters)
    {
        _countText.SetText(ExperienceSystem.Instance.CurrentPerksCount.ToString());
    }
}