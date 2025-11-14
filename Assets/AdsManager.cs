using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

[RequireComponent(typeof(RewardedAds))]
public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] bool _testMode;
    [SerializeField] string _playstoreID = "";
    [SerializeField] string _playstoreRewardedID = "Rewarded_Android";
    [SerializeField] RewardedAds _playstoreRewardedScript;
    //[SerializeField] string _playstoreInterstitialID = "";
    //[SerializeField] string _playStoreBannerID = "";

    public static AdsManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_playstoreID, _testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialization Complete.");
        _playstoreRewardedScript.Initialized(_playstoreRewardedID);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void ShowMyRewardedAd(IWantReward giveRewardTo)
    {
        _playstoreRewardedScript.ShowRewardedAd(giveRewardTo);
    }
}
