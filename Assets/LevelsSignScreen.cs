using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsSignScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] GlobalStamina _globalStamina;
    [SerializeField] int _stamineCost = 100;
    [SerializeField] TextMeshProUGUI _levelTxt;
    [SerializeField] Button _closeButton;
    [SerializeField] Button _playButton;
    [SerializeField] string _sceneName = "Mobile Lv1 PauseManager";
    string _actualLevelName;

    void Awake()
    {
        _closeButton.onClick.AddListener(() => ScreenManager.Instance.DeactivateScreen());
        _playButton.onClick.AddListener(GoToLevel);
    }

    public void UpdateLevelSign(string lvName, string internalLvName)
    {
        _levelTxt.text = ($"Enter {lvName}?");
        _actualLevelName = internalLvName;
    }

    public void GoToLevel()
    {
        if (_globalStamina.UseStamina(_stamineCost))
        {
            ScreenManager.Instance.DeactivateScreen();
            SceneTransition.Instance.LoadLevel(_actualLevelName);
        }
    }

    public void Activate()
    {
        _root.SetActive(true);
    }

    public void Deactivate()
    {
        _root.SetActive(false);
    }
}