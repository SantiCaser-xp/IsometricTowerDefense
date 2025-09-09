using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTowerBullet : AbstractBullet
{
    IDamageable<float> target;
    private Transform targetTransform;
    public float speed = 40f;
    public int damage = 10;

    private void Update()
    {
        if (_isShooted && (targetTransform == null || target == null))
        {
            _myPool.Release(this);
            return;
        }

        Vector3 dir = (targetTransform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    public override void SetTarget(IDamageable<float> newTarget, Transform targetTf)
    {
        target = newTarget;
        targetTransform = targetTf;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform == targetTransform)
        {
            HitTarget();
        }
    }

    public void HitTarget()
    {
        target.TakeDamage(damage);
        _myPool.Release(this);
    }

    public override void Refresh()
    {
        target = null;
        targetTransform = null;
        _isShooted = false;
    }
}
