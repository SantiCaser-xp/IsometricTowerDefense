using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] GameObject _pauseScreen;
    Button _pauseButton;

    private void Awake()
    {
        _pauseButton = GetComponent<Button>();
    }
    
    private void Start()
    {
        _pauseButton.onClick.AddListener(() => ScreenManager.Instance.ActivateScreen(_pauseScreen));
    }
}