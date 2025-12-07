using TMPro;
using UnityEngine;

public class LevelRewardsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _perksCount;

    void OnEnable()
    {
        ActualPerks();    
    }

    void ActualPerks(params object[] args)
    {
        _perksCount.SetText($"+{ExperienceSystem.Instance.PerkCounterAtLevel}");
    }
}