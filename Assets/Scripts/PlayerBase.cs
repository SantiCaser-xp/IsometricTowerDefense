using Unity.Services.RemoteConfig;
using UnityEngine;

public class PlayerBase : Destructible
{
    [SerializeField] TargetType _targetType;
    [SerializeField] Animator _animator;

    private void Start()
    {
        InitializeBase();
    }

    public void InitializeBase()
    {
        _currentHealth = PerkSkillManager.Instance.StartHealth;
        Debug.Log("Player Base Health at Start: " + _currentHealth);

        RemoteConfigService.Instance.FetchCompleted += SetHealthFromRemote;

        EnemyTargetManager.Instance?.RegisterTarget(this);

        foreach (var obs in _observers)
        {
            obs.UpdateData(_currentHealth, PerkSkillManager.Instance.StartHealth);
            Debug.Log("Player Base Health Initialized");
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