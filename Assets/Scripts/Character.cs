using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Character : MonoBehaviour, IRestoreable, IDamageable
{
    public static Action<bool> OnDead;
    [SerializeField] private float _delayInSeconds = 900f; 
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth = 0f;
    private bool _isAlive = true;
    

    private CharacterInputController _controller;
    private CharacterMovement _movement;

    private void Awake()
    {
        _controller = new CharacterInputController();
        _movement = GetComponent<CharacterMovement>();
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
        }
    }

    public void RestoreHealth(float value)
    {
        if (value <= 0f) return;

        _currentHealth += value;

        if(_currentHealth >= _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = _maxHealth;
            Die();
        }
    }

    public void Die()
    {
        _isAlive = false;
        OnDead?.Invoke(_isAlive);
    }
}