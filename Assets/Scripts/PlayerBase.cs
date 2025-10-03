using Unity.Services.RemoteConfig;
using UnityEngine;

public class PlayerBase : Destructible
{
    [SerializeField] TargetType _targetType;
    [SerializeField] Animator _animator;

    public event System.Action OnPlayerBaseDestroyed;

    private void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += UpdateData;
        ITargetable targetable = this.GetComponent<ITargetable>();
        if (targetable != null)
        {
            EnemyTargetManager.Instance?.RegisterTarget(targetable);
        }

        _currentHealth = _maxHealth;

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, _maxHealth);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _animator.SetTrigger("Hit");
    }

    public override void Die()
    {
        OnPlayerBaseDestroyed?.Invoke();
        _animator.SetTrigger("Die");
    }

    public void UpdateData(ConfigResponse configResponse)
    {
        _currentHealth = RemoteConfigService.Instance.appConfig.GetInt("SetTentHp");

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, _maxHealth);
        }
    }
}
