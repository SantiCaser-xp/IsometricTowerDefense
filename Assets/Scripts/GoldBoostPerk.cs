using UnityEngine;

public class GoldBoostPerk : Perk, IPerkEffect<int>
{
    [SerializeField] private int _goldBoostPerUpgrade = 5;
    private int _currentBoost = 0;
    public int CurrentBoost => _currentBoost;

    public int RemoveEffect()
    {
        _currentBoost -= _goldBoostPerUpgrade;
        return _currentBoost;
    }

    public int ApplyEffect()
    {
        _currentBoost += _goldBoostPerUpgrade;
        return _currentBoost;
    }

    protected override void OnPerkApplied()
    {
        Debug.Log("5 gold in Start Deposit");
    }
}
