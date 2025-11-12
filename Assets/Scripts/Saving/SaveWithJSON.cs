using UnityEngine;
using System.IO;

public class SaveWithJSON : MonoBehaviour
{
    [SerializeField] protected SaveData _saveData = new SaveData();
    [SerializeField] protected string _pathBase = "";
    [SerializeField] protected string _path = "";

    private void Awake()
    {
#if UNITY_EDITOR
        _pathBase = Application.dataPath;
#else
        _pathBase = Application.persistentDataPath;
#endif

        _path = _pathBase + "/SaveData.Json"; //name of saveData
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SaveGame();       
        if (Input.GetKeyDown(KeyCode.Space)) LoadGame();       
    }

    public void SaveGame()
    {
        var json = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(_path, json);
        //PlayerPrefs.SetString("MyJson", json);
    }

    public void LoadGame()
    {
        if(Directory.Exists(_pathBase) && File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            JsonUtility.FromJsonOverwrite(json, _saveData);
        }

        //if(PlayerPrefs.HasKey("MyJson"))
        //{
        //    string json = PlayerPrefs.GetString("MyJson");
        //    JsonUtility.FromJsonOverwrite(json, _saveData);
        //}
    }

    public void DeleteAll()
    {
        File.Delete(_path);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus) SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause) SaveGame();
    }
}