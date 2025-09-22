using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string _levelName = "";
    private LevelManager _levelManager;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(StartLevel);
    }

    private void Start()
    {
        _levelManager = LevelManager.Instance;
    }


    private void StartLevel()
    {
        _levelManager.LoadLevel(_levelName);
    }
}