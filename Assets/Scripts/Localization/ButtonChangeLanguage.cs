using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeLanguage : MonoBehaviour
{
    [SerializeField] Language language;
    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(BTNChangeLanguage);
    }


    public void BTNChangeLanguage()
    {
        LocalizationManager.Instance.ChangeLanguage(language);
    }
}