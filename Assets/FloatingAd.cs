using UnityEngine;
using UnityEngine.UI;


public class FloatingAd : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] ShowRewardScreen showRewardScreen;

    private void Start()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {

        ScreenManager.Instance.ActivateScreen(showRewardScreen);
        gameObject.SetActive(false);
    }

}
