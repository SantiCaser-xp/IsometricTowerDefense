using System;
using UnityEngine;

[Serializable]
public class PerkSkillManager : SingltonBase<PerkSkillManager>
{
    [SerializeField] float _startCharacterSpeed = 5;
    public float StarCharacterSpeed => _startCharacterSpeed;

    [SerializeField] int _startGold = 5;
    public int StartGold => _startGold;

    [SerializeField] float _startHealth = 100f;
    public float StartHealth => _startHealth;

    [SerializeField] float _startRadius = 5f;
    public float StartRadius => _startRadius;

    [SerializeField] float _startRocketDamage = 10f;
    public float StartRocketDamage => _startRocketDamage;

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        if (SaveWithJSON.Instance == null) return;

        var sd = SaveWithJSON.Instance._perksData;

        sd.CharacterSpeed = _startCharacterSpeed;
        sd.Gold = _startGold;
        sd.Health = _startHealth;

        SaveWithJSON.Instance._perksData = sd;
        SaveWithJSON.Instance.SaveGame();
    }

    public void Load()
    {
        var sd = SaveWithJSON.Instance._perksData;

        _startCharacterSpeed = sd.CharacterSpeed;
        _startGold = sd.Gold;
        _startHealth = sd.Health;
    }

    public void ChangeCharacterSpeed(float speed)
    {
        _startCharacterSpeed += speed;
    }

    public void ChangeGold(int gold)
    {
        _startGold += gold;
    }

    public void ChangeHealth(float health)
    {
        _startHealth += health;
    }

    public void ChangeRocketRadius(float radius)
    {
        _startRadius += radius;
    }

    public void ChangeRocketDamage(float damage)
    {
        _startRocketDamage += damage;
    }
}