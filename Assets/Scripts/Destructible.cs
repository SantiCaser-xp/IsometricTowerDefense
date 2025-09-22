using UnityEngine;

public abstract class Destructible : MonoBehaviour, IDamageable<float>, IKillable, ITargetable
{
    [SerializeField] protected float _maxHealth = 100f;
    protected float _currentHealth = 0f;

    public TargetType TargetType => TargetType.Tower;

    public virtual void Die()
    {

    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);

        if (_currentHealth <= 0f)
        {
            Die();
        }

    }
}