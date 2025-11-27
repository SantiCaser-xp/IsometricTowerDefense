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

    public void ChangeCharacterSpeed(float speed)
    {
        _startCharacterSpeed *= speed;
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
        _startRocketDamage += radius;
    }

    public void ChangeRocketDamage(float damage)
    {
        _startRocketDamage += damage;
    }
}