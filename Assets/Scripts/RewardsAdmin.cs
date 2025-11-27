using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsAdmin : MonoBehaviour
{
    // Start is called before the first frame update
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
                case RewardType.InitialCoins:
                    Debug.Log("Player rewarded with Initial Coins");
                    ApplyCoinsReward();
                    break;
                case RewardType.StaminaBoost:
                    Debug.Log("Player rewarded with Stamina");
                    break;
                default:
                    Debug.Log("Unknown reward type");
                    break;
            }
        }
        
    }

    public void ApplyCoinsReward()
    {
        PerkSkillManager.Instance.ChangeGold(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
