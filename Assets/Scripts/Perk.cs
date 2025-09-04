using UnityEngine;

public abstract class Perk : MonoBehaviour, IResettable<int>
{
    [SerializeField] protected int _maxPrice = 3;
    [SerializeField] protected int _maxUpgradeLevel = 3;
    [SerializeField, TextArea] protected string _description;
    protected int _currentUpgradeLevel = 1;
    protected int _currentPrice = 1;

    protected virtual void RecalculateUpgradeLevel()
    {
        if(_currentPrice < _maxPrice && _currentUpgradeLevel < _maxUpgradeLevel)
        {
            _currentUpgradeLevel++;
            _currentPrice++;
        }
    }

    public void Reset()
    {
        _currentUpgradeLevel = 1;
        _currentPrice = 1;
    }
}