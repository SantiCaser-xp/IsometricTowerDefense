using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverSystem : MonoBehaviour, IObserver
{
    [Header("Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Buttons (lose)")]
    [SerializeField] private Button loseRestartBtn;
    [SerializeField] private Button loseMenuBtn;

    [Header("Buttons (win)")]
    [SerializeField] private Button winNextBtn;
    [SerializeField] private Button winRestartBtn;
    [SerializeField] private Button winMenuBtn;

    [Header("UI to hide on GameOver")]
    [SerializeField] private List<GameObject> gameplayUI = new List<GameObject>();

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [SerializeField] private LvManager lvManager;

    public EnemySpawnerLvSystem spawner;

    public static bool IsGameOver { get; private set; }

    void Awake()
    {
        HideAll();

        if (loseRestartBtn) loseRestartBtn.onClick.AddListener(RestartLevel);
        if (loseMenuBtn) loseMenuBtn.onClick.AddListener(LoadMainMenu);

        if (winNextBtn) winNextBtn.onClick.AddListener(LoadNextLevel);
        if (winRestartBtn) winRestartBtn.onClick.AddListener(RestartLevel);
        if (winMenuBtn) winMenuBtn.onClick.AddListener(LoadMainMenu);

        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        obs.Subscribe(this);
    }

    private void Start()
    {
        spawner.AllWavesCleared += HandleWavesCleared;
    }
    void OnDestroy()
    {
        // Es buena práctica desuscribirse
        spawner.AllWavesCleared -= HandleWavesCleared;
    }

    private void HandleWavesCleared()
    {
        UpdateGameStatus(GameStatus.Win);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        SetGroupActive(gameplayUI, true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        SetGroupActive(gameplayUI, true);

        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
        else
            SceneManager.LoadScene(0);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        SetGroupActive(gameplayUI, true);

        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            LoadMainMenu();
    }

    private void HideAll()
    {
        IsGameOver = false;
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
    }

    private void SetGroupActive(List<GameObject> list, bool state)
    {
        if (list == null) return;
        foreach (var go in list) if (go) go.SetActive(state);
    }

    public void UpdateData(float currentValue, float maxValue) { }

    public void UpdateData(int value) { }

    public void UpdateGameStatus(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.Win:
                if (IsGameOver) return;
                IsGameOver = true;

                SetGroupActive(gameplayUI, false);
                Time.timeScale = 0f;

                if (winPanel)
                {
                    winPanel.SetActive(true);
                    winPanel.transform.SetAsLastSibling();
                }
                break;

            case GameStatus.Lose:
                if (IsGameOver) return;
                IsGameOver = true;

                SetGroupActive(gameplayUI, false);
                Time.timeScale = 0f;

                if (losePanel)
                {
                    losePanel.SetActive(true);
                    losePanel.transform.SetAsLastSibling();
                }
                break;
        }
    }
}
