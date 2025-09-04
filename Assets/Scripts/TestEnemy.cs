using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, I_TestDamageable
{
    private Rigidbody rb;
    [SerializeField] private float hp = 100f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector3.left * Time.fixedDeltaTime * 2);
    }

    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
