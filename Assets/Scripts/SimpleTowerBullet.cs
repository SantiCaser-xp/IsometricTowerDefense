using UnityEngine;

public class SimpleTowerBullet : AbstractBullet
{
    IDamageable<float> target;
    private Transform targetTransform;
    public float speed = 40f;
    public int damage = 10;
    private float _timer = 1f;

    private void OnEnable()
    {
        Invoke(nameof(OnRelease), _timer);
    }

    private void Update()
    {
        Vector3 dir = transform.forward;
        transform.position += dir * speed * Time.deltaTime;
    }

    //private void Update()
    //{
    //    if (_isShooted && (targetTransform == null || target == null))
    //    {
    //        _myPool.Release(this);
    //        return;
    //    }
        //Vector3 dir = (targetTransform.position - transform.position).normalized;
        //transform.position += dir* speed * Time.deltaTime;
    //    
    //}

    //public override void SetTarget(IDamageable<float> newTarget, Transform targetTf)
    //{
    //    target = newTarget;
    //    targetTransform = targetTf;
    //}

    public void OnTriggerEnter(Collider other)
    {
        Destructible dest = other.GetComponent<Destructible>();

        if (dest != null)
        {
            target.TakeDamage(damage);
            OnRelease();
            
        }
    }

    private void OnRelease()
    {
        _myPool.Release(this);
    }

    //public override void Refresh()
    //{
    //    target = null;
    //    targetTransform = null;
    //    _isShooted = false;
    //}
}
