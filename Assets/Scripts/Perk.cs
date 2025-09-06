using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Perk : MonoBehaviour, IResettable<int>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected int _maxPrice = 3;
    [SerializeField] protected int _maxUpgradeLevel = 3;
    [SerializeField, TextArea] protected string _description;
    [SerializeField] private PerkPoints _points;
    [SerializeField] private Tooltip _tooltip;
    protected int _currentUpgradeLevel = 1;
    protected int _currentPrice = 1;

    public void BuyPerk()
    {
        if(_points.TryUsePerk(_currentPrice))
        {
            RecalculateUpgradeLevel();
            OnPerkApplied();
        }
    }

    protected virtual void RecalculateUpgradeLevel()
    {
        if(_currentPrice < _maxPrice && _currentUpgradeLevel < _maxUpgradeLevel)
        {
            _currentUpgradeLevel++;
            _currentPrice++;
        }
    }

    protected abstract void OnPerkApplied();

    public void Reset()
    {
        _currentUpgradeLevel = 1;
        _currentPrice = 1;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        _tooltip.ShowTooltip(Input.mousePosition, _description);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.HideTooltip();
    }
}