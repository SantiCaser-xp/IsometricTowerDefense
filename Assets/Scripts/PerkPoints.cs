using System;
using UnityEngine;

public class PerkPoints : MonoBehaviour, IResettable<int>
{
    public static Action<int> OnPerksChanged;

    [Header("Perk points")]
    [SerializeField] private int _perkPerLevel = 1;
    private int _availablePerks = 0;
    public int AvailablePerks => _availablePerks;

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
        _availablePerks += _perkPerLevel;
        OnPerksChanged?.Invoke(_availablePerks);
    }

    public bool TryUsePerk(int price)
    {
        if (_availablePerks >= price)
        {
            _availablePerks -= price;
            OnPerksChanged?.Invoke(_availablePerks);
            return true;
        }

        return false;
    }

    public void Reset()
    {
        _availablePerks = 0;
    }
}