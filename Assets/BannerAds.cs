using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class BannerAds : MonoBehaviour
{
    string _id;

    private void Start()
    {
        EventManager.Subscribe(EventType.SceneChanged, HideBannerAd);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.SceneChanged, HideBannerAd);
    }

    public void Initialize(string id)
    {
        _id = id;

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

        StartCoroutine(CicleBanner());
    }

    IEnumerator CicleBanner()
    {
        while (true)
        {
            LoadBannerAd();
            yield return new WaitForSeconds(5f);
            ShowBannerAd();
            yield return new WaitForSeconds(30f);
            HideBannerAd();
            yield return new WaitForSeconds(10f);
        }
    }

    private void ShowBannerAd()
    {
        if (SceneManager.GetActiveScene().name != "WorldMap")
            return;

        BannerOptions options = new BannerOptions
        {
            clickCallback = OnClickBanner,
            hideCallback = OnHideBanner,
            showCallback = OnShowBanner
        };
        Advertisement.Banner.Show(_id, options);
    }


    private void OnShowBanner()
    {
        Debug.Log("Banner ad shown");
        EventManager.Trigger(EventType.OnBannerAdShown);
    }

    private void OnHideBanner()
    {
        Debug.Log("Banner ad hidden");
        EventManager.Trigger(EventType.OnBannerAdDisable);
    }

    private void OnClickBanner()
    {
        Debug.Log("Banner ad clicked");
    }

    private void HideBannerAd(params object[] parameters) => Advertisement.Banner.Hide();

    private void LoadBannerAd()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnLoadedBannerAd,
            errorCallback = OnBannerError
        };
        Advertisement.Banner.Load(_id, options);
    }

    private void OnBannerError(string message)
    {
        Debug.Log("Banner ad error: " + message);
    }

    private void OnLoadedBannerAd()
    {
        Debug.Log("Banner ad loaded");
    }
}
