using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string _internalLevelName = "";
    [SerializeField] private string _levelName = "";
    private Button _button;
    [SerializeField] private bool _bttnAlreadyPushedThisSession = false;
    [SerializeField] LevelsSign _levelSign;
    [SerializeField] GameObject _levelSignGO;
    [SerializeField] GameObject _adSign;

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
            _adSign.SetActive(true);
            _levelSign.UpdateLevelSign(_levelName, _internalLevelName);

            // Aquí podrías poner lógica adicional, como mostrar un anuncio
        }
        else
        {
            ShowLvPannel();
        }
    }

    public void ShowLvPannel()
    {
        _levelSign.UpdateLevelSign(_levelName, _internalLevelName);

        _levelSignGO.SetActive(true);
    }
}