using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] GameObject _settingsScreen;
    [SerializeField] Button _resumeButton;
    [SerializeField] Button _tutorialButton;
    [SerializeField] Button _settingsButton;
    [SerializeField] Button _exitGameButton;

    void Awake()
    {
        _resumeButton.onClick.AddListener(() => ScreenManager.Instance.DeactivateScreen());
        _tutorialButton.onClick.AddListener(OpenTutorial);
        _settingsButton.onClick.AddListener(() => ScreenManager.Instance.ActivateScreen(_settingsScreen));
        _exitGameButton.onClick.AddListener(ExitGame);
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

    /*public void OpenPausePC()
    {
        ScreenManager.Instance.ActivateScreen(this);
    }*/

    public void OpenTutorial()
    {
        ScreenManager.Instance.DeactivateScreen();
        SceneTransition.Instance.LoadLevel("Tutorial");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        
        EditorApplication.isPlaying = false;
#else
        
        Application.Quit();
#endif
    }
}