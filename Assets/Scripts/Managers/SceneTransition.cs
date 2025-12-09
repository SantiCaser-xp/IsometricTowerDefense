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
    string _currentLevelToLoad;
    Coroutine _fadeInCoroutine;
    Coroutine _fadeOutCoroutine;
    Coroutine _loadCoroutine;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        _progressPanel.SetActive(false);
        _progressBar.fillAmount = 0;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_fadeOutCoroutine != null)
            StopCoroutine(_fadeOutCoroutine);

        _fadeOutCoroutine = StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeInAndLoad(string sceneName)
    {
        _fadeInCoroutine = StartCoroutine(FadeInRoutine());
        yield return _fadeInCoroutine;

        yield return StartCoroutine(LoadSceneAsync(sceneName));
    }

    void CanLoadLevel(bool v) => _operation.allowSceneActivation = v;

    public void LoadLevel(string name)
    {
        if (_isLoading) return;
        _isLoading = true;

        if (_fadeOutCoroutine != null)
            StopCoroutine(_fadeOutCoroutine);

        if (_fadeInCoroutine != null)
            StopCoroutine(_fadeInCoroutine);

        if (_loadCoroutine != null)
            StopCoroutine(_loadCoroutine);

        EventManager.Trigger(EventType.SceneChanged);

        _loadCoroutine = StartCoroutine(FadeInAndLoad(name));
    }

    public void AskForAd(string sceneName)
    {
        _currentLevelToLoad = sceneName;
    }

    IEnumerator FadeInRoutine()
    {
        //_blackPanel.raycastTarget = true;
        float elapsed = 0f;
        Color color = _blackPanel.color;
        float startAlpha = color.a;

        while (elapsed < _fadeInTime)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 1f, elapsed / _fadeInTime);
            _blackPanel.color = color;
            yield return null;
        }

        color.a = 1f;
        _blackPanel.color = color;
    }

    IEnumerator FadeOutRoutine()
    {
        float elapsed = 0f;
        Color color = _blackPanel.color;
        float startAlpha = color.a;

        while (elapsed < _fadeOutTime)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 0f, elapsed / _fadeOutTime);
            _blackPanel.color = color;
            yield return null;
        }

       // _blackPanel.raycastTarget = false;
        color.a = 0f;
        _blackPanel.color = color;
    }

    IEnumerator LoadSceneAsync(string sceneName)
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