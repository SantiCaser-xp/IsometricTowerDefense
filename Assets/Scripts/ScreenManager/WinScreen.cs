using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] Button _nextButton;
    [SerializeField] Button _returnButton;

    private void Awake()
    {
        _nextButton.onClick.AddListener(NextLevel);
        _returnButton.onClick.AddListener(ReturnToMap);
    }

    public void NextLevel()
    {
        ScreenManager.Instance.DeactivateScreen();
        SceneTransition.Instance.LoadLevel("WorldMap");
    }

    public void ReturnToMap()
    {
        ScreenManager.Instance.DeactivateScreen();
        SceneTransition.Instance.LoadLevel("WorldMap");
    }

    public void Activate()
    {
        _root.SetActive(true);
        PauseManager.Instance.Pause(this);
    }

    public void Deactivate()
    {
        _root.SetActive(false);
        PauseManager.Instance.Unpause(this);
    }
}