using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject menuWindow;
    [SerializeField] private Button pauseOverlay;

    [Header("Settings")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button settingsCloseX;
    [SerializeField] private Button settingsBack;

    [Header("UI to hide on Pause")]
    [SerializeField] private List<GameObject> gameplayUI = new List<GameObject>();

    public static bool IsPaused { get; private set; }

    void Awake()
    {
        if (pauseOverlay != null)
        {
            pauseOverlay.onClick.RemoveAllListeners();
            pauseOverlay.onClick.AddListener(Resume);
        }

        if (settingsCloseX != null)
        {
            settingsCloseX.onClick.RemoveAllListeners();
            settingsCloseX.onClick.AddListener(CloseSettings);
        }
        if (settingsBack != null)
        {
            settingsBack.onClick.RemoveAllListeners();
            settingsBack.onClick.AddListener(CloseSettings);
        }

        var overlayBtn = settingsPanel ? settingsPanel.transform.Find("SettingsOverlay") : null;
        var overlayButton = overlayBtn ? overlayBtn.GetComponent<Button>() : null;
        if (overlayButton != null)
        {
            overlayButton.onClick.RemoveAllListeners();
            overlayButton.onClick.AddListener(CloseSettings);
        }

        if (settingsPanel) settingsPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel != null && settingsPanel.activeSelf) CloseSettings();
            else if (IsPaused) Resume();
        }
    }

    public void OpenPause()
    {
        if (GameOverSystem.IsGameOver) return;
        if (IsPaused) return;
        IsPaused = true;

        if (pausePanel) pausePanel.SetActive(true); else Debug.LogWarning("[PAUSE] pausePanel not assigned");
        if (menuWindow) menuWindow.SetActive(true); else Debug.LogWarning("[PAUSE] menuWindow not assigned");
        if (settingsPanel) settingsPanel.SetActive(false);

        SetGroupActive(gameplayUI, false);

        Time.timeScale = 0f;
        AudioManager.Instance?.MuteNonMusicDuringPause(true);
    }

    public void Resume()
    {
        if (!IsPaused) return;
        IsPaused = false;

        if (settingsPanel) settingsPanel.SetActive(false);
        if (menuWindow) menuWindow.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);

        SetGroupActive(gameplayUI, true);

        Time.timeScale = 1f;
        AudioManager.Instance?.MuteNonMusicDuringPause(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.MuteNonMusicDuringPause(false);
        IsPaused = false;

        SetGroupActive(gameplayUI, true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenSettings()
    {
        if (menuWindow) menuWindow.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel) settingsPanel.SetActive(false);
        if (menuWindow) menuWindow.SetActive(true);
    }

    private void SetGroupActive(List<GameObject> list, bool state)
    {
        if (list == null) return;
        foreach (var go in list) if (go) go.SetActive(state);
    }
}
