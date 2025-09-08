using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTowerBullet : MonoBehaviour
{
    I_TestDamageable target;
    private Transform targetTransform;
    public float speed = 40f;
    public int damage = 10;
    public System.Action OnBulletExpired;

    private void Update()
    {
        if (targetTransform == null || target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (targetTransform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        //float distanceThisFrame = speed * Time.deltaTime;


        //transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        //transform.rotation = Quaternion.LookRotation(direction);
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

    private void Expire()
    {
        OnBulletExpired?.Invoke();
    }
}
