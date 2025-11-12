using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
//all objects what have text to translate must have this script
public class TextTranslate : MonoBehaviour
{
    [SerializeField] string _ID;
    TextMeshProUGUI _txt;

    private void Awake()
    {
        _txt = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        LocalizationManager.Instance.EventTranslate += Translate;
        Translate();
    }

    private void Start()
    {
        //LocalizationManager.Instance.EventTranslate += Translate;
        Translate();
    }

    private void OnDisable()
    {
        LocalizationManager.Instance.EventTranslate -= Translate;
    }

    private void Translate()
    {
        _txt.text = LocalizationManager.Instance.GetTranslate(_ID);
    }
}