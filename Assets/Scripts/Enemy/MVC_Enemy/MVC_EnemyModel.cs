using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MVC_EnemyModel
{
    //data
    private float _maxHealth;
    private float _currentHealth;
    private EnemyData _data;
    private ITargetable _currentTarget;

    //flocking settings
    private Vector3 _currentSeparationForce;
    private float _nextFlockUpdateTime;
    private Vector3 _smoothVelocity;

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
        _currentHealth = _maxHealth;

        _nextFlockUpdateTime = Time.time + UnityEngine.Random.Range(0f, 0.2f);
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
        MonoBehaviour mb = damageable as MonoBehaviour;

        if (mb.GetComponent<Destructible>().CurrentHealth <= 0) return;

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

    public void UpdateFlocking()
    {
        if (!_agent.enabled || !_agent.isOnNavMesh) return;

        if (Time.time > _nextFlockUpdateTime)
        {
            CalculateSeparationForce();
            _nextFlockUpdateTime = Time.time + _data.flockUpdateInterval;
        }

        Vector3 targetVelocity = _agent.desiredVelocity;
        targetVelocity += _currentSeparationForce * _data.separationWeight;
        _agent.velocity = Vector3.Lerp(_agent.velocity, targetVelocity, Time.deltaTime * 5f);
    }
    private void CalculateSeparationForce()
    {
        Vector3 separationTotal = Vector3.zero;
        int count = 0;

        var enemies = EnemyManager.Instance.ActiveEnemies;
        float sqrRadius = _data.separationRadius * _data.separationRadius;
        Vector3 myPos = _transform.position;

        for (int i = 0; i < enemies.Count; i++)
        {
            var other = enemies[i];
            if(other == null||other.Model==this) continue;

            Vector3 otherPos = other.GetPos();
            Vector3 diff = myPos - otherPos ;
            float sqrDist = diff.sqrMagnitude;

            if (sqrDist < sqrRadius && sqrDist > 0.01f)
            {
                separationTotal += diff.normalized / Mathf.Sqrt(sqrDist);
                count++;
            }
        }
        if (count > 0) {
            separationTotal /= count;
            _currentSeparationForce = separationTotal;
        }
        else
        {
            _currentSeparationForce = Vector3.zero;
        }
    }
}