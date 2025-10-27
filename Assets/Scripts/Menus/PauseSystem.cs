using UnityEngine;
using UnityEngine.UI;

public class PauseSystem : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject _mainPausePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;

    [Header("Buttons")]
    [SerializeField] Button _pauseButton;
    [SerializeField] Button _restarButton;
    [SerializeField] Button _resumeButton;
    [SerializeField] Button _settingsButton;
    [SerializeField] Button _settingsCloseButton;
    [SerializeField] Button _exitButton;

    public static bool IsPaused { get; private set; }

    private void Start()
    {
        _restarButton.onClick.AddListener(SceneTransition.Instance.RestartLevel);
        _exitButton.onClick.AddListener(() => SceneTransition.Instance.LoadLevel("WorldMap"));
        _resumeButton.onClick.AddListener(Resume);
        _settingsButton.onClick.AddListener(OpenSettings);
        _settingsCloseButton.onClick.AddListener(CloseSettings);
        _pauseButton.onClick.AddListener(OpenPause);

        _settingsPanel.SetActive(false);
        _mainPausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_settingsPanel.activeSelf) CloseSettings();
            else if (IsPaused) Resume();
        }
    }

    public void OpenPause()
    {
        if (GameOverSystem.IsGameOver) return;
        if (IsPaused) return;
        IsPaused = true;

        if (_mainPausePanel) _mainPausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (!IsPaused) return;
        IsPaused = false;

        if (_mainPausePanel) _mainPausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        if (_pausePanel) _pausePanel.SetActive(false);
        if (_settingsPanel) _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (_settingsPanel) _settingsPanel.SetActive(false);
        if (_pausePanel) _pausePanel.SetActive(true);
    }
}