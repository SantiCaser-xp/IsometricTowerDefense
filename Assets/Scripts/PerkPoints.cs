using System;
using UnityEngine;

public class PerkPoints : MonoBehaviour, IResettable<int>
{
    public static Action<int> OnPerksChanged;

    [Header("Perk points")]
    [SerializeField] private int _perkPerLevel = 1;
    private int _currentPerks = 0;

    private void OnEnable()
    {
        LevelSystem.OnLevelChanged += OnLevelUp;
    }

    private void OnDisable()
    {
        LevelSystem.OnLevelChanged -= OnLevelUp;
    }

    private void OnLevelUp(int newLevel)
    {
        AddPerk();
    }

    private void AddPerk()
    {
        _currentPerks += _perkPerLevel;
        OnPerksChanged?.Invoke(_currentPerks);
    }

    public bool TryUsePerk(int price)
    {
        if (_currentPerks >= price)
        {
            _currentPerks -= price;
            OnPerksChanged?.Invoke(_currentPerks);
            return true;
        }

        return false;
    }

    public void Reset()
    {
        _currentPerks = 0;
    }
}