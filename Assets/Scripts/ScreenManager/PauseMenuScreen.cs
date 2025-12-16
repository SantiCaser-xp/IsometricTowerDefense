using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] GameObject _settingsScreen;
    [SerializeField] GameObject _tutorialScreen;
    [SerializeField] GameObject _exitConfirmScreen;
    [SerializeField] Button _resumeButton;
    [SerializeField] Button _tutorialButton;
    [SerializeField] Button _settingsButton;
    [SerializeField] Button _exitGameButton;

    void Awake()
    {
        _resumeButton.onClick.AddListener(() => ScreenManager.Instance.DeactivateScreen());
        _tutorialButton.onClick.AddListener(() => ScreenManager.Instance.ActivateScreen(_tutorialScreen));
        _settingsButton.onClick.AddListener(() => ScreenManager.Instance.ActivateScreen(_settingsScreen));
        _exitGameButton.onClick.AddListener(() => ScreenManager.Instance.ActivateScreen(_exitConfirmScreen));
    }

    public void Activate()
    {
        PauseManager.Instance.Pause(this);
        _root.SetActive(true);
    }

    public void Deactivate()
    {
        PauseManager.Instance.Unpause(this);
        _root.SetActive(false);
    }
}