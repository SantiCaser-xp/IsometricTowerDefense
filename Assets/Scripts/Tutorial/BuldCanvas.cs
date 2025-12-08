using UnityEngine;
using UnityEngine.UI;

public class BuldCanvas : MonoBehaviour
{
    [SerializeField] Button _buildButton;
    [SerializeField] Button _shieldButton;
    [SerializeField] bool _tutorialMode = true;

    void Awake()
    {
        if (_tutorialMode)
        {
            _buildButton.gameObject.SetActive(false);
            _shieldButton.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        EventManager.Subscribe(EventType.FirstGoldCatched, ShowShieldButton);
        EventManager.Subscribe(EventType.ZoomCamera, ShowBuildButton);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(EventType.FirstGoldCatched, ShowShieldButton);
        EventManager.Unsubscribe(EventType.ZoomCamera, ShowBuildButton);
    }

    void ShowShieldButton(params object[] args)
    {
        _shieldButton.gameObject.SetActive(true);
    }

    void ShowBuildButton(params object[] args)
    {
        _buildButton.gameObject.SetActive(true);
    }
}