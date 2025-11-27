using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveWithJSON : SingltonBase<SaveWithJSON>
{
    //public SaveData _saveData = new SaveData();
    public StaminaSaveData _staminaData = new StaminaSaveData();
    public SettingsSaveData _settingsData = new SettingsSaveData();
    public ExperienceSaveData _experienceData = new ExperienceSaveData();
    public PerksSaveData _perksData = new PerksSaveData();

    [SerializeField] protected string _pathBase = "";
    //[SerializeField] protected string _path = "";
    [SerializeField] protected string _pathStamina = "";
    [SerializeField] protected string _pathExperience = "";
    [SerializeField] protected string _pathPerks = "";
    [SerializeField] protected string _pathSettings = "";

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        _pathBase = Application.dataPath;
#else
        _pathBase = Application.persistentDataPath;
#endif

        _pathStamina = _pathBase + "/StaminaData.Json";
        _pathExperience = _pathBase + "/ExpData.Json";
        _pathPerks = _pathBase + "/PeksData.Json";
        _pathSettings = _pathBase + "/SettingsData.Json";

        SceneManager.sceneLoaded += OnSceneChanged;
        LoadGame();
    }

    private void OnSceneChanged(Scene arg0, LoadSceneMode arg1)
    {
        LoadGame();
    }

    public void SaveGame()
    {
        var json = JsonUtility.ToJson(_staminaData, true);
        File.WriteAllText(_pathStamina, json);

        var json1 = JsonUtility.ToJson(_experienceData, true);
        File.WriteAllText(_pathExperience, json1);

        var json2 = JsonUtility.ToJson(_perksData, true);
        File.WriteAllText(_pathPerks, json2);

        var json3 = JsonUtility.ToJson(_settingsData, true);
        File.WriteAllText(_pathSettings, json3);
    }

    public void LoadGame()
    {
        if (!File.Exists(_pathStamina))
        {
            _staminaData = new StaminaSaveData();
            SaveGame();
            return;
        }

        if (!File.Exists(_pathExperience))
        {
            _experienceData = new ExperienceSaveData();
            SaveGame();
            return;
        }
        if (!File.Exists(_pathPerks))
        {
            _perksData = new PerksSaveData();
            SaveGame();
            return;
        }

        if (!File.Exists(_pathSettings))
        {
            _settingsData = new SettingsSaveData();
            SaveGame(); 
            return;
        }

        if (Directory.Exists(_pathBase) && File.Exists(_pathStamina))
        {
            string json = File.ReadAllText(_pathStamina);
            JsonUtility.FromJsonOverwrite(json, _staminaData);
        }

        if (Directory.Exists(_pathBase) && File.Exists(_pathExperience))
        {
            string json = File.ReadAllText(_pathExperience);
            JsonUtility.FromJsonOverwrite(json, _experienceData);
        }

        if (Directory.Exists(_pathBase) && File.Exists(_pathPerks))
        {
            string json = File.ReadAllText(_pathPerks);
            JsonUtility.FromJsonOverwrite(json, _perksData);
        }

        if (Directory.Exists(_pathBase) && File.Exists(_pathSettings))
        {
            string json = File.ReadAllText(_pathSettings);
            JsonUtility.FromJsonOverwrite(json, _settingsData);
        }
    }

    public void DeleteAll()
    {
        if (File.Exists(_pathStamina))
            File.Delete(_pathStamina);
        if (File.Exists(_pathExperience))
            File.Delete(_pathExperience);
        if (File.Exists(_pathPerks))
            File.Delete(_pathPerks);
        if (File.Exists(_pathSettings))
            File.Delete(_pathSettings);

        _staminaData = new StaminaSaveData();
        _settingsData = new SettingsSaveData();
        _experienceData = new ExperienceSaveData();
        _perksData = new PerksSaveData();

        Debug.Log("Deleted save file.");
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus) SaveGame();
    }
}