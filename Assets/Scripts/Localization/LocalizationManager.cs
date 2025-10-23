using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;
    public Language language;
    public DataLocalization[] data;

    Dictionary<Language, Dictionary<string, string>> _translate = new();
    public event Action EventTranslate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _translate = LanguageU.LoadTranslate(data);
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    public string GetTranslate(string id)
    {
        if (!_translate.ContainsKey(language))
        {
            return "No Language";
        }
        if (!_translate[language].ContainsKey(id))
        {
            return "No ID";
        }
        return _translate[language][id];
    }
    public void ChangeLanguage(Language newLanguage)
    {
        if (language == newLanguage) return;
        language = newLanguage;
        EventTranslate?.Invoke();
    }
    private void Update()
    {

    }
}
