using UnityEngine;

public class PerkPanelUI : MonoBehaviour
{
    [SerializeField] private PerkManager _manager;
    [SerializeField] private PerkUI _perkUIPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private PerkData[] _perks;

    private void Start()
    {
        foreach (var perk in _perks)
        {
            var perkUI = Instantiate(_perkUIPrefab, _container);
            perkUI.Setup(perk, _manager);
        }
    }
}