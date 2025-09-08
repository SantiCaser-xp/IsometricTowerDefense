using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBullet : MonoBehaviour
{
    protected ObjectPool<AbstractBullet> _myPool;
    [SerializeField] public float _damage;
    public void Initialize(ObjectPool<AbstractBullet> pool)
    {
        _myPool = pool;
    }

    public virtual void Refresh() { }

    //protected virtual void OnTriggerEnter(Collider other)
    //{
    //    var damageable = other.GetComponent<I_TestDamageable>();

    //    if (damageable != null)
    //    {
    //        damageable.TakeDamage(_damage);
    //        _myPool.Release(this);
    //    }
    //}
}
