using UnityEngine;
using TMPro;


[RequireComponent(typeof(TextMeshProUGUI))]
//all objects what have text to translate must have this script
public class TextTranslate : MonoBehaviour
{
    [SerializeField] string _ID;
    TextMeshProUGUI _txt;

    void Start()
    {
        _txt = GetComponent<TextMeshProUGUI>();

        LocalizationManager.Instance.EventTranslate += Translate;

        Translate();
    }

    private void Translate()
    {
        _txt.text = LocalizationManager.Instance.GetTranslate(_ID);
    }

    private void OnDestroy()
    {
        LocalizationManager.Instance.EventTranslate += Translate;
    }
}
