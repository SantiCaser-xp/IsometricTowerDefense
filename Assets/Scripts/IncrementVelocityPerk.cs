using UnityEngine;

public class IncrementVelocityPerk : Perk, IPerkEffect<float>
{
    [SerializeField] private float _speedBoostPerUpgrade = 0.1f;
    private float _currentBoost = 1.0f;
    public float CurrentBoost => _currentBoost;

    protected override void OnPerkApplied()
    {
        _currentBoost += _speedBoostPerUpgrade;
        Debug.Log(_currentBoost);
    }

    public float ApplyEffect()
    {
        return _currentBoost;
    }

    public float RemoveEffect()
    {
        return _currentBoost -= _speedBoostPerUpgrade;
    }
}
