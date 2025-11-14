using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRewardSign : MonoBehaviour, IWantReward
{
    public void GiveReward()
    {
        Debug.Log("Rewarded Ad Finished - Give Reward to Player");
    }

    public void ShowAd()
    {
        AdsManager.Instance.ShowMyRewardedAd(this);
        gameObject.SetActive(false);
    }
}
