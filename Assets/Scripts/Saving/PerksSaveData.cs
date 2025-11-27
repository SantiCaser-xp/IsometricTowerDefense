using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PerksSaveData
{
    [Header("Shop")]
    public List<PerkData> BoughtPerks = new List<PerkData>();

    [Header("Skills")]
    public float CharacterSpeed = 5;
    public int Gold = 5;
    public float Health = 100f;
}