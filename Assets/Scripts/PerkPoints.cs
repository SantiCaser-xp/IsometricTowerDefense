using System;
using UnityEngine;

public class PerkPoints : MonoBehaviour, IResettable<int>
{
    public static Action<int> OnPerksChanged;
    public static PerkPoints Instance;

    [Header("Perk points")]
    [SerializeField] private int _perkPerLevel = 1;
    private int _availablePerks = 0;
    public int AvailablePerks => _availablePerks;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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