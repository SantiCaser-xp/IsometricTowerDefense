using UnityEngine;
using UnityEngine.UI;

public class DeleteConfirmScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;

    void Awake()
    {
        _yesButton.onClick.AddListener(OnDeleteDataButtonClicked);
        _noButton.onClick.AddListener(() => ScreenManager.Instance.DeactivateScreen());
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

    void OnDeleteDataButtonClicked()
    {
        SaveWithJSON.Instance.DeleteAll();
        ScreenManager.Instance.DeactivateAll();
        SceneTransition.Instance.ExitFromGame();
    }
}