using UnityEngine;

public class PlayerBase : Destructible
{
    [SerializeField] TargetType _targetType;

    private void Start()
    {
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
}
