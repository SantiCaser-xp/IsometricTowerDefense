using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public float CurrentExperience;
    public int CurrentStamine;
    public int CurrentLevel;
    public int CurrentPerks;

    [Header("Settings")]
    public Language Language;
    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;
    public float UIVolume;
}