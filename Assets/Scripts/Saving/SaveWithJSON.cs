using UnityEngine;
using System.IO;

public class SaveWithJSON : SingltonBase<SaveWithJSON>
{
    public SaveData _saveData = new SaveData();
    [SerializeField] protected string _pathBase = "";
    [SerializeField] protected string _path = "";
    private bool _deleted = false;

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        _pathBase = Application.dataPath;
#else
        _pathBase = Application.persistentDataPath;
#endif

        _path = _pathBase + "/SaveData.Json"; //name of saveData
        LoadGame();
    }

    public void SaveGame()
    {
        var json = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(_path, json);
    }

    public void LoadGame()
    {
        if (!File.Exists(_path))
        {
            _saveData = new SaveData();
            SaveGame();
            return;
        }

        if (Directory.Exists(_pathBase) && File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            JsonUtility.FromJsonOverwrite(json, _saveData);
        }
    }

    public void DeleteAll()
    {
        _deleted = true;

        if (File.Exists(_path))
            File.Delete(_path);

        Debug.Log("Deleted save file.");
    }

    private void OnApplicationQuit()
    {
        if (!_deleted)
            SaveGame();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && !_deleted)
            SaveGame();
    }
}