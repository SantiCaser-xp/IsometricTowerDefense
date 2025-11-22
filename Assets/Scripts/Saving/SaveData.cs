using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    [Header("Experience System")]
    public float CurrentExperience;
    public float CurrentExperienceThreshold;
    public int CurrentLevel;
    public int CurrentPerks;

    [Header("Global Stamine")]
    public int CurrentStamina;
    public string NextStaminaTime;
    public string LastStaminaTime;

    [Header("Settings")]
    public Language Language;
    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;
    public float UIVolume;

    [Header("Skills")]
    public PerkSkillsData MyPerkSkillData;

    public SaveData()
    {
        InitializeDefaultData();
    }

    public void InitializeDefaultData()
    {
        CurrentExperience = 0f;
        CurrentLevel = 1;
        CurrentPerks = 0;
        CurrentStamina = 100;
        Language = Language.ENG;
        MasterVolume = 1f;
        MusicVolume = 1f;
        SFXVolume = 1f;
        UIVolume = 1f;
        MyPerkSkillData = new PerkSkillsData();
    }
}