using System;
using UnityEngine;
using UnityEngine.Windows;

public class SaveWithCustomJSon : SaveWithJSON
{
    protected virtual void Awake()
    {
        _pathBase = Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
        Application.companyName + "/" + Application.productName;

        if(!Directory.Exists(_pathBase)) Directory.CreateDirectory(_pathBase);

        _path = _pathBase + "/SaveData";

        LoadGame();
    }
}
