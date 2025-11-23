using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : SingltonBase<SceneTransition>
{
    [SerializeField] Image _blackPanel;
    [SerializeField] GameObject _progressPanel;
    [SerializeField] float _fadeInTime = 1f;
    [SerializeField] float _fadeOutTime = 2f;
    [SerializeField] Image _progressBar;
    AsyncOperation _operation;
    bool _isLoading = false;
    private string _currentLevelToLoad;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        _progressPanel.SetActive(false);
        _progressBar.fillAmount = 0;

        //StartCoroutine(FadeOutRoutine());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeInAndLoad(string sceneName)
    {
        yield return StartCoroutine(FadeInRoutine());
        yield return StartCoroutine(LoadSceneAsync(sceneName));
    }

    void CanLoadLevel(bool v) => _operation.allowSceneActivation = v;

    public void LoadLevel(string name)
    {
        if (_isLoading) return;
        _isLoading = true;
        StartCoroutine(FadeInAndLoad(name));
    }

    public void AskForAd(string sceneName)
    {
        _currentLevelToLoad = sceneName;
    }

    private IEnumerator FadeInRoutine()
    {
        float elapsed = 0f;
        Color color = _blackPanel.color;

        while (elapsed < _fadeInTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(0f + elapsed / _fadeInTime);
            color.a = alpha;
            _blackPanel.color = color;
            yield return null;
        }

        color.a = 1f;
        _blackPanel.color = color;
    }

    private IEnumerator FadeOutRoutine()
    {
        float elapsed = 0f;
        Color color = _blackPanel.color;

        while (elapsed < _fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - elapsed / _fadeOutTime);
            color.a = alpha;
            _blackPanel.color = color;
            yield return null;
        }

        color.a = 0f;
        _blackPanel.color = color;
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        _progressPanel.SetActive(true);

        _operation = SceneManager.LoadSceneAsync(sceneName);

        CanLoadLevel(false);
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        while (!_operation.isDone)
        {
            float progress = Mathf.Clamp01(_operation.progress / 0.9f);
            yield return new WaitForEndOfFrame();
            _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, progress, Time.deltaTime);

            if (_progressBar.fillAmount >= 1)
            {
                Application.backgroundLoadingPriority = ThreadPriority.High;
                CanLoadLevel(true);
            }

            yield return null;
        }

        _progressPanel.SetActive(false);
        
        _isLoading = false;
    }

    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void ExitFromGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}