using Unity.Services.RemoteConfig;
using UnityEngine;

public class PlayerBase : Destructible
{
    [SerializeField] TargetType _targetType;
    [SerializeField] Animator _animator;

    private void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += SetHealthFromRemote;

        EnemyTargetManager.Instance?.RegisterTarget(this);

        _currentHealth = PerkSkillManager.Instance.StartHealth;

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, PerkSkillManager.Instance.StartHealth);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _animator.SetTrigger("Hit");
    }

    public override void Die()
    {
        EnemyTargetManager.Instance.UnregisterTarget(this);
        _animator.SetTrigger("Die");
        Debug.Log("Base Destroyed");
        EventManager.Trigger(EventType.OnGameOver);//add delay before gameoverscreen
    }

    void SetHealthFromRemote(ConfigResponse configResponse)
    {
        _currentHealth += RemoteConfigService.Instance.appConfig.GetInt("TentHpAdd");
        UpdateData();
    }

    public void UpdateData(params object[] values)
    {
        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, PerkSkillManager.Instance.StartHealth);
        }
    }
}