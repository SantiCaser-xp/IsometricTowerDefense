using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    string _id;
    private RewardType _rewardType;

    public void Initialized(string id)
    {
        _id = id;
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(_id, this);
    }

    public void ShowRewardedAd(RewardType rewardType)
    {
        _rewardType = rewardType;
        //Debug.Log("Reward Type set to: " + _rewardType.ToString());
        Advertisement.Show(_id, this);
    }




    public void OnUnityAdsAdLoaded(string placementId)
    {
        //Debug.Log("Ad Loaded Successfully.");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        //Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
        //LoadRewardedAd();
    }




    public void OnUnityAdsShowClick(string placementId)
    {
        //Debug.Log("Ad Clicked.");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == _id)
        {
            if(showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                //Debug.Log("Unity Ads Rewarded Ad Completed");
                //Debug.Log("Rewarding player with: " + _rewardType.ToString());
                EventManager.Trigger(EventType.OnAdFinished, _rewardType);

                // Reward the user for watching the ad to completion.
            }
            else if (showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED))
            {
                //Debug.Log("Unity Ads Rewarded Ad Skipped");
                // Do not reward the user for skipping the ad.
            }
            else if (showCompletionState.Equals(UnityAdsShowCompletionState.UNKNOWN))
            {
                //Debug.Log("Unity Ads Rewarded Ad Unknown");
            }
        }
        LoadRewardedAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        LoadRewardedAd();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //Debug.Log("Ad Started.");
    }
}
