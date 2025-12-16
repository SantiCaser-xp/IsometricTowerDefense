using UnityEngine;
using UnityEngine.UI;

public class ExitFromLevelScreen : MonoBehaviour, IScreen
{
    [SerializeField] GameObject _root;
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;

    void Awake()
    {
        _noButton.onClick.AddListener(() => ScreenManager.Instance.DeactivateScreen());
        _yesButton.onClick.AddListener(ReturnToMenu);
    }

    public void Activate()
    {
        _root.SetActive(true);
    }

    public void Deactivate()
    {
        _root.SetActive(false);
    }

    public void ReturnToMenu()
    {
        ScreenManager.Instance.DeactivateAll();
        SceneTransition.Instance.LoadLevel("WorldMap");
    }
}