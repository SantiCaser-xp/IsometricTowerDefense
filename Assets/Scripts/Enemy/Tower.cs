using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, ITargetable
{
    [SerializeField] private float maxHealth = 200f;
    [SerializeField] private float currentHealth;

    public Vector3 Position => transform.position;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsAlive => currentHealth > 0;
    public TargetType TargetType => TargetType.Tower;
    public GameObject GameObject => gameObject;

    private void Start()
    {
        currentHealth = maxHealth;
        EnemyTargetManager.Instance?.RegisterTarget(this);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DestroyTower();
        }

        // ќбновл€ем UI здоровь€
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // Ћогика обновлени€ UI
    }

    private void DestroyTower()
    {
        EnemyTargetManager.Instance?.UnregisterTarget(this);

        // Ёффекты разрушени€
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EnemyTargetManager.Instance?.UnregisterTarget(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(20);
        }
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }
}
