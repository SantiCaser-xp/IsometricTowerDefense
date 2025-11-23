using UnityEngine;
using UnityEngine.UI;

public class PauseLevelScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] GameObject _settingsScreen;
    [SerializeField] Button _restarButton;
    [SerializeField] Button _resumeButton;
    [SerializeField] Button _settingsButton;
    [SerializeField] Button _returnToMapButton;

    private void Awake()
    {
        _restarButton.onClick.AddListener(RestartLevel);
        _resumeButton.onClick.AddListener(() => ScreenManager.Instance.DeactivateScreen());
        _settingsButton.onClick.AddListener(() => ScreenManager.Instance.ActivateScreen(_settingsScreen));
        _returnToMapButton.onClick.AddListener(ReturnToMenu);
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

    public void OpenPausePC()
    {
        ScreenManager.Instance.ActivateScreen(this);
    }

    public void RestartLevel()
    {
        ScreenManager.Instance.DeactivateScreen();
        SceneTransition.Instance.RestartLevel();
    }

    public void ReturnToMenu()
    {
        ScreenManager.Instance.DeactivateScreen();
        SceneTransition.Instance.LoadLevel("WorldMap");
    }
}