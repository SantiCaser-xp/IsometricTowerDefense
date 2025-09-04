using System.Collections;
using UnityEngine;

public class RealtimeValuesManager : MonoBehaviour//, IRestoreable, IDamageable
{
    #region Singlton
    public static RealtimeValuesManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _currentHealth = _maxHealth;
    }

    #endregion

    [SerializeField] private float _delayInSeconds = 900f; //15 minutes
    [SerializeField] private float _amountPerInterval = 5f;
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth = 0f;
    private Coroutine _currentCoroutine;

    private void Start()
    {
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(nameof(RestoreHealthRoutine));
        }
    }

    /*public void RestoreHealth(float value)
    {
        
    }

    public void TakeDamage(float damage)
    {
        
    }

    public void Die()
    {
        
    }*/

    /// <summary>
    /// This is the method for generate health as tries for play.(Limit)
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestoreHealthRoutine()
    {
        while (_currentHealth < _maxHealth)
        {
            yield return new WaitForSeconds(_delayInSeconds);

            _currentHealth += _amountPerInterval;
            _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

            yield return null;
        }
    }

    public void SubstructHealth(float value)
    {
        if (value <= 0f) return;

        _currentHealth -= value;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);


    }
}