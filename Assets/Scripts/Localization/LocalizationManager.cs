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

    private void Start()
    {
        Load();
        EventTranslate?.Invoke();
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
        Save();
        EventTranslate?.Invoke();
        //print("Language changed!");
    }

    void Save()
    {
        var sd = SaveWithJSON.Instance._settingsData;

        sd.Language = language;
        SaveWithJSON.Instance._settingsData = sd;
        SaveWithJSON.Instance.SaveGame();
    }

    void Load()
    {
        var savedLang = SaveWithJSON.Instance._settingsData.Language;
        ChangeLanguage(savedLang);
    }
}