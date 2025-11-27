using UnityEngine;
using UnityEngine.UI;

public class ShowRewardScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] GameObject _levelScreen;
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;
    void Awake()
    {
        _yesButton.onClick.AddListener(ShowAd);
        _noButton.onClick.AddListener(NextScreen);
    }

    public void Activate()
    {
        _root.SetActive(true);
    }

    public void Deactivate()
    {
        _root.SetActive(false);
    }

    public void ShowAd()
    {
        //AdsManager.Instance.ShowMyRewardedAd(this);
        AdsManager.Instance.ShowMyRewardedAd(RewardType.StaminaBoost);
        ScreenManager.Instance.DeactivateScreen();
    }

    private void NextScreen()
    {
        ScreenManager.Instance.DeactivateScreen();
        ScreenManager.Instance.ActivateScreen(_levelScreen);
    }
}