using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject _tooltip;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Vector3 _tooltipOffset;

    private void Awake()
    {
        HideTooltip();
    }

    public void ShowTooltip(Vector3 pos, string description)
    {
        _tooltip.SetActive(true);
        _tooltip.transform.position = pos + _tooltipOffset;
        _description.text = description;
    }

    public void HideTooltip()
    {
        _tooltip.SetActive(false);
    }
}