using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExitFromGameScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;

    void Awake()
    {
        _noButton.onClick.AddListener(() => ScreenManager.Instance.DeactivateScreen());
        _yesButton.onClick.AddListener(ExitGame);
    }

    public void Activate()
    {
        _root.SetActive(true);
    }

    public void Deactivate()
    {
        _root.SetActive(false);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        ScreenManager.Instance.DeactivateAll();
        EditorApplication.isPlaying = false;
#else
        
        Application.Quit();
#endif
    }

}
