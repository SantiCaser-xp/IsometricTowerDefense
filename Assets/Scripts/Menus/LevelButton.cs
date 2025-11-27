using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string _internalLevelName = "";
    [SerializeField] private string _levelName = "";
    [SerializeField] LevelsSignScreen _levelSign;
    [SerializeField] ShowRewardScreen _showStaminaAdScreen;
    [SerializeField] GlobalStamina _globalStamina;

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
        if (_globalStamina.CurrentStamina < 25f)
        {
            ScreenManager.Instance.ActivateScreen(_showStaminaAdScreen);
            return;
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