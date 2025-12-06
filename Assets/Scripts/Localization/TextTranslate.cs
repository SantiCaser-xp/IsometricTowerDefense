using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
//all objects what have text to translate must have this script
public class TextTranslate : MonoBehaviour
{
    [SerializeField] string _ID;
    TextMeshProUGUI _txt;

    void Awake()
    {
        _txt = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        LocalizationManager.Instance.EventTranslate += Translate;
        Translate();
    }

    void Start()
    {
        Translate();
    }

    void OnDisable()
    {
        LocalizationManager.Instance.EventTranslate -= Translate;
    }

    void Translate()
    {
        _txt.text = LocalizationManager.Instance.GetTranslate(_ID);
    }
}