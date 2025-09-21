using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menuBlock;
    [SerializeField] private GameObject background;

    [Header("Settings Buttons")]
    [SerializeField] private Button settingsCloseX;
    [SerializeField] private Button settingsBack;

    [Header("Load Target")]
    [SerializeField] private string gameSceneName = "";
    [SerializeField] private int gameSceneBuildIndex = -1;

    void Awake()
    {
        if (playButton) playButton.onClick.AddListener(PlayGame);
        if (settingsButton) settingsButton.onClick.AddListener(OpenSettings);
        if (exitButton) exitButton.onClick.AddListener(ExitGame);

        if (settingsCloseX) settingsCloseX.onClick.AddListener(CloseSettings);
        if (settingsBack) settingsBack.onClick.AddListener(CloseSettings);

        if (settingsPanel) settingsPanel.SetActive(false);

        Time.timeScale = 1f;
        AudioManager.Instance?.MuteNonMusicDuringPause(false);
    }

    public void OpenSettings()
    {
        if (menuBlock) menuBlock.SetActive(false);
        if (settingsPanel)
        {
            settingsPanel.SetActive(true);
            settingsPanel.transform.SetAsLastSibling();
        }

    }

    public void CloseSettings()
    {
        if (settingsPanel) settingsPanel.SetActive(false);
        if (menuBlock) menuBlock.SetActive(true);

    }

    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
            SceneManager.LoadScene(gameSceneName);
        else if (gameSceneBuildIndex >= 0)
            SceneManager.LoadScene(gameSceneBuildIndex);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
