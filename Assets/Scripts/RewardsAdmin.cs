using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsAdmin : MonoBehaviour
{
    [SerializeField] GlobalStamina _globalStamina;
    void Start()
    {
        EventManager.Subscribe(EventType.OnAdFinished, OnAdFinishedd);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnAdFinished, OnAdFinishedd);
    }

    private void OnAdFinishedd(object rewardTypeObj)
    {
        if (rewardTypeObj != null)
        {
            if (rewardTypeObj is object[] arr && arr.Length > 0)
            {
                rewardTypeObj = arr[0];
            }

            RewardType rewardType = (RewardType)rewardTypeObj;
            switch (rewardType)
            {
                case RewardType.AddExperience:
                    Debug.Log("Player rewarded with Experience");
                    AddExperience();
                    break;
                case RewardType.StaminaBoost:
                    AddStamina();
                    Debug.Log("Player rewarded with Stamina");
                    break;
                case RewardType.AddPerksPoint:
                    AddPerk();
                    Debug.Log("Player rewarded with perkPoints");
                    break;
                default:
                    Debug.Log("Unknown reward type");
                    break;
            }
        }
        
    }

    public void AddExperience()
    {
        ExperienceSystem.Instance.AddExperience(5f);
        ExperienceSystem.Instance.Save();
    }

    public void AddStamina()
    {
        _globalStamina.AddStamina(25);
    }
    public void AddPerk()
    {
        ExperienceSystem.Instance.AddPerk();
        ExperienceSystem.Instance.Save();
    }
}
