using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;

public class MVC_EnemyModel
{
    //data
    private float _maxHealth;
    private float _currentHealth;
    private EnemyData _data;
    private ITargetable _currentTarget;

    //components
    private Transform _transform;
    private NavMeshAgent _agent;

    //observing
    private List<IObserver> _observers = new List<IObserver>();

    //Events
    public event Action<float, float> OnHealthChanged; //current,Max
    public event Action OnTakeDamage;
    public event Action OnAttack;
    public event Action OnDie;
    public event Action<bool> OnSetMoving;

    //Propirties
    public ITargetable CurrentTarget => _currentTarget;
    public Vector3 Position => _transform.position;
    public EnemyData Data => _data;

    //constructor
    public MVC_EnemyModel(NavMeshAgent agent, Transform transform, EnemyData data, float maxHealth)
    {
        _agent = agent;
        _transform = transform;
        _data = data;
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }


    public void SetTarget(ITargetable target)
    {
        _currentTarget = target;
    }
    public void MoveTo(Vector3 destination)
    {
        if (!_agent.enabled || !_agent.isOnNavMesh) return;

        _agent.SetDestination(destination);
        _agent.isStopped = false;
        OnSetMoving?.Invoke(true);
    }
    public void StopMovement()
    {
        if (!_agent.enabled || !_agent.isOnNavMesh) return;

        _agent.isStopped = true;
        OnSetMoving?.Invoke(false);
    }
    public void PerformAttack()
    {
        if (_currentTarget == null) return;

        IDamageable<float> damageable = _currentTarget as IDamageable<float>;
        if (damageable != null)
        {
            damageable.TakeDamage(_data.damage);
            OnAttack?.Invoke();
        }
    }
    public void RotateTowardTarget()
    {
        if (_currentTarget == null) return;

        Vector3 lookDirection = (_currentTarget.GetPos() - _transform.position).normalized;
        lookDirection.y = 0;
        _transform.rotation = Quaternion.LookRotation(lookDirection);
    }
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        OnTakeDamage?.Invoke();

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, _maxHealth);
        }
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        OnDie?.Invoke();
    }
    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
    public Vector3 GetPos()
    {
        return _transform.position;
    }
    public void Subscribe(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            observer.UpdateData(_currentHealth, _maxHealth);
        }
    }
    public void Unsubscribe(IObserver observer)
    {
        _observers.Remove(observer);

    }
}
