using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestEnemy : MonoBehaviour, I_TestDamageable
{
    private Rigidbody rb;
    [SerializeField] private float hp = 100f;
    [SerializeField] private Transform ringPosition;

    public event System.Action<I_TestDamageable> OnDeath;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   

    private void FixedUpdate()
    {
        //rb.MovePosition(rb.position + Vector3.left * Time.fixedDeltaTime * 2);
    }

    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(this);

        Destroy(gameObject);
    }


}
