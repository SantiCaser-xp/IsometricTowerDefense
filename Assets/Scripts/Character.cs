using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Character : MonoBehaviour, IRestoreable, IDamageable, IObservable
{
    public static Action<bool> OnDead;
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth = 0f;
    private bool _isAlive = true;
    [SerializeField] private CharacterMeshRotator _meshRotator;
    [SerializeField] private ControlBase _joystick;
    private CharacterInputController _controller;
    private CharacterMovement _movement;
    private List<IObserver> _observers = new List<IObserver>();

    private void Awake()
    {
        _controller = new CharacterInputController(_joystick);
        _movement = GetComponent<CharacterMovement>();

        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, _maxHealth);
        }
    }

    private void Update()
    {
        _controller.InputArtificialUpdate();
    }

    private void FixedUpdate()
    {
        if(_controller.InputDirection.sqrMagnitude > 0.001f)
        {
            _movement.Movement(_controller.InputDirection);
            _meshRotator.RotateMesh(_controller.InputDirection);
        }
    }

    public void RestoreHealth(float value)
    {
        if (value <= 0f) return;

        _currentHealth += value;

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, _maxHealth);
        }

        if (_currentHealth >= _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f) return;

        _currentHealth -= damage;

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, _maxHealth);
        }

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        _isAlive = false;
        OnDead?.Invoke(_isAlive);
    }

    public void Subscribe(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Unsubscribe(IObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
        }
    }

    #region ForTest
    [ContextMenu("TakeDamage")]
    private void GetDamage()
    {
        TakeDamage(10);
    }
    #endregion
}