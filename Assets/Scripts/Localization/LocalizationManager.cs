using System;
using System.Collections.Generic;

public class LocalizationManager : SingltonBase<LocalizationManager>
{
    public Language language;
    public DataLocalization[] data;

    Dictionary<Language, Dictionary<string, string>> _translate = new();
    public event Action EventTranslate;

    protected override void Awake()
    {
        base.Awake();

        _translate = LanguageU.LoadTranslate(data);
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
}