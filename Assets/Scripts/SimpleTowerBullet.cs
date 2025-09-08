using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTowerBullet : AbstractBullet
{
    I_TestDamageable target;
    private Transform targetTransform;
    public float speed = 40f;
    public int damage = 10;

    private void Update()
    {
        if (targetTransform == null || target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (targetTransform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    public void SetTarget(I_TestDamageable newTarget, Transform targetTf)
    {
        target = newTarget;
        targetTransform = targetTf;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == targetTransform)
        {
            HitTarget();
        }
    }

    public void HitTarget()
    {
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}
