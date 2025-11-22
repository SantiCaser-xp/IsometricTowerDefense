using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string _internalLevelName = "";
    [SerializeField] private string _levelName = "";
    [SerializeField] private bool _bttnAlreadyPushedThisSession = false;
    [SerializeField] LevelsSignScreen _levelSign;
    [SerializeField] ShowRewardScreen showRewardScreen;

    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (!_bttnAlreadyPushedThisSession)
        {
            _bttnAlreadyPushedThisSession = true;
            ScreenManager.Instance.ActivateScreen(showRewardScreen);
        }
        else
        {
            ShowLvPannel();
        }
    }

    public void ShowLvPannel()
    {
        _levelSign.UpdateLevelSign(_levelName, _internalLevelName);
        ScreenManager.Instance.ActivateScreen(_levelSign);
    }
}