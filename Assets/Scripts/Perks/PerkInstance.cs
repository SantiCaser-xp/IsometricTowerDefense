using System;
using UnityEngine;

public class PerkInstance
{
    public event Action<PerkInstance> OnChanged;

    [SerializeField] private PerkData _data;
    [SerializeField] protected int _currentLevel;
    [SerializeField] protected int _currentPrice;
    public PerkData Data => _data;
    public int CurrentLevel => _currentLevel;
    public int CurrentPrice => _currentPrice;

    public PerkInstance(PerkData data)
    {
        _data = data;
        Reset();
    }

    public bool TryUpgrade()
    {
        if (_currentLevel >= _data.MaxUpgradeLevel) return false;
        if (!ExperienceSystem.Instance.TryUsePerk(_currentPrice)) return false;

        _currentLevel++;
        _currentPrice = Mathf.Min(_currentPrice + 1, _data.MaxPrice);

        OnChanged?.Invoke(this);

        return true;
    }

    public void Reset()
    {
        _currentLevel = _data.BaseLevel;
        _currentPrice = _data.BasePrice;
        OnChanged?.Invoke(this);
    }
}