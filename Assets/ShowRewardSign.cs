using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRewardSign : MonoBehaviour
{
    public void ShowAd()
    {
        AdsManager.Instance.ShowMyRewardedAd();
        gameObject.SetActive(false);
    }
}
