using System;

[Serializable]
public class SaveData
{
    public SaveData _saveData = new SaveData();
    public StaminaSaveData _staminaData = new StaminaSaveData();
    public SettingsSaveData _settingsData = new SettingsSaveData();
    public ExperienceSaveData _experienceData = new ExperienceSaveData();
    public PerksSaveData _perksData = new PerksSaveData();
    public bool FirstLauch = true;
}
