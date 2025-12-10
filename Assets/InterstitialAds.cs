using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    string _id;
    [SerializeField] private int _clickedBttnCounter;

    public void Initialized(string id)
    {
        _id = id;
        LoadInterstitialAd();
    }

    private void Awake()
    {
        EventManager.Subscribe(EventType.ShowInterstitialAd, PlayInterstitialAd);
        EventManager.Subscribe(EventType.ButtonForInterstitialClicked, CountButtonClicks);
    }


    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.ShowInterstitialAd, PlayInterstitialAd);
        EventManager.Unsubscribe(EventType.ButtonForInterstitialClicked, CountButtonClicks);
    }

    private void CountButtonClicks(params object[] parameters)
    {
        _clickedBttnCounter++;
        if (_clickedBttnCounter >= 7)
        {
            _clickedBttnCounter = 0;
            ShowInterstitialAd();
        }
    }

    private void PlayInterstitialAd(params object[] parameters)
    {
        ShowInterstitialAd();
    }

    public void LoadInterstitialAd()
    {
        Advertisement.Load(_id, this);
    }

    public void ShowInterstitialAd()
    {
        Advertisement.Show(_id, this);
    }




    public void OnUnityAdsAdLoaded(string placementId)
    {
        //Debug.Log("Ad Loaded Successfully.");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        //Debug.Log($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");
        LoadInterstitialAd();
    }




    public void OnUnityAdsShowClick(string placementId)
    {
        //Debug.Log("Ad Clicked.");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        LoadInterstitialAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        LoadInterstitialAd();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //Debug.Log("Ad Started.");
    }
}
