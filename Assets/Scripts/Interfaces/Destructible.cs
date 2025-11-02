using System.Collections.Generic;
using UnityEngine;

public abstract class Destructible : MonoBehaviour, IDamageable<float>, IKillable, ITargetable, IObservable
{
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _currentHealth = 0f;
    protected List<IObserver> _observers = new List<IObserver>();
    public TargetType TargetType => TargetType.Tower;

    public virtual void Die()
    {

    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, _maxHealth);
        }

        if (_currentHealth <= 0f)
        {
            Die();
        }

    }

    public virtual void Subscribe(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public virtual void Unsubscribe(IObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
        }
    }
}