using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Character : MonoBehaviour, IRestoreable, IDamageable<float>, IObservable, ITargetable
{
    public static Action<bool> OnDead;
    public static Action<float, float> OnHealthChanged;
    
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth = 0f;
    private bool _isAlive = true;
    
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;

    public TargetType TargetType => TargetType.PlayerBase;

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
        
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        //ITargetable targetable = this.GetComponent<ITargetable>();
        //if (targetable != null)
        //{
        //    EnemyTargetManager.Instance?.RegisterTarget(targetable);
        //}
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
        
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
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
        
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void Die()
    {
        _isAlive = false;
        OnDead?.Invoke(_isAlive);
        FindObjectOfType<GameOverSystem>(true).UpdateGameStatus(GameStatus.Lose);
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

    public Vector3 GetPos()
    {
        return transform.position;
    }
    #endregion
}