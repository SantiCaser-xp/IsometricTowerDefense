using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsSignScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] GlobalStamine _globalStamine;
    [SerializeField] int _stamineCost = 100;
    [SerializeField] TextMeshProUGUI _levelTxt;
    [SerializeField] Button _closeButton;
    [SerializeField] Button _playButton;
    string _actualLevelName;

    private void Awake()
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
        if (_globalStamine.UseStamina(_stamineCost))
        {
            ScreenManager.Instance.DeactivateScreen();
            SceneTransition.Instance.LoadLevel("Mobile Lv1 PauseManager");
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