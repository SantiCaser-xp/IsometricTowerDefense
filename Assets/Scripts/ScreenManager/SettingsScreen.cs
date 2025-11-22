using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] Button _settingsCloseButton;

    void Awake()
    {
        _settingsCloseButton.onClick.AddListener(() =>
            ScreenManager.Instance.DeactivateScreen());
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