using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    [SerializeField] private List<PerkData> _perkDataList;

    private Dictionary<PerkData, PerkInstance> _instances = new();

    private void Awake()
    {
        foreach (var data in _perkDataList)
            _instances[data] = new PerkInstance(data);
    }

    public PerkInstance GetInstance(PerkData data)
    {
        return _instances[data];
    }

    public bool BuyPerk(PerkData data)
    {
        if (_instances.TryGetValue(data, out var instance))
            return instance.TryUpgrade();

        return false;
    }
}