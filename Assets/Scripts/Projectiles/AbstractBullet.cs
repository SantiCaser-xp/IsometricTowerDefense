using UnityEngine;

public abstract class AbstractBullet : MonoBehaviour
{
    protected ObjectPool<AbstractBullet> _myPool;
    [SerializeField] protected float _damage= 10f;
    [SerializeField] protected bool _isShooted = false;
    [SerializeField] protected float speed = 40f;
    protected IDamageable<float> _target;
    protected Transform _targetTransform;
    float timer = 0;


    protected void Update()
    {

        timer += Time.deltaTime;
        

        if (timer > 3)
        {

            Debug.Log("Release");
            _myPool.Release(this);
            return;
        }

        if (_isShooted && (_targetTransform == null || _target == null))
        {
            _myPool.Release(this);
            return;
        }

        MoveProjectile();
    }

    public void Initialize(ObjectPool<AbstractBullet> pool)
    {
        _myPool = pool;
    }

    public virtual void Refresh()
    {
        _target = null;
        _targetTransform = null;
        _isShooted = false;
        timer = 0;
    }

    public virtual void SetTarget(IDamageable<float> newTarget, Transform targetTf)
    {
        _target = newTarget;
        _targetTransform = targetTf;
        _isShooted = true;
    }

    protected virtual void MoveProjectile()
    {
        Vector3 dir = (_targetTransform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.transform == _targetTransform)
        {
            _target.TakeDamage(_damage);
            _myPool.Release(this);
        }
    }

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
