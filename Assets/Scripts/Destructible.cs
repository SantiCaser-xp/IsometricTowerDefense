using System;
using UnityEngine;

public abstract class Destructible : MonoBehaviour, IDamageable<float>, IKillable
{
    [SerializeField] protected float _maxHealth = 100f;
    protected float _currentHealth = 0f;

    public event Action OnDead;

    public virtual void Die()
    {
        //logic
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

        if (_currentHealth <= 0f)
        {
            Die();
        }

    }
}
