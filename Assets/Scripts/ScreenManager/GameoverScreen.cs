using UnityEngine;
using UnityEngine.UI;

public class GameoverScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] Button _restarButton;
    [SerializeField] Button _returnButton;

    private void Awake()
    {
        _restarButton.onClick.AddListener(RestartLevel);
        _returnButton.onClick.AddListener(ReturnToMap);
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

    public void ReturnToMap()
    {
        ScreenManager.Instance.DeactivateScreen();
        SceneTransition.Instance.LoadLevel("WorldMap");
    }

    public void RestartLevel()
    {
        ScreenManager.Instance.DeactivateScreen();
        SceneTransition.Instance.RestartLevel();
    }
}