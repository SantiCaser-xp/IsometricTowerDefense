using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGreanade : MonoBehaviour, IObservable
{
    protected Vector3 startPos;
    protected Vector3 targetPos;
    protected float height;
    protected float duration;
    protected float t;
    [SerializeField] protected float radius = 5f;
    [SerializeField] protected LayerMask _enemyMask;
    [SerializeField] protected Animator _anim;
    protected ObjectPool<AbstractGreanade> _myPool;
    [SerializeField] protected GameObject mesh;
    private List<IObserver> _observers = new List<IObserver>();

    public void Initialize(ObjectPool<AbstractGreanade> pool)
    {
        _myPool = pool;
    }

    private void Update()
    {
        if (t < 1)
        {
            t += Time.deltaTime / duration;

            // Movimiento parabólico
            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);
            pos.y += height * Mathf.Sin(Mathf.PI * t); // parábola simple
            transform.position = pos;

            // Rotar hacia el movimiento
            Vector3 dir = (pos - transform.position).normalized;
            if (dir != Vector3.zero)
                transform.forward = dir;
        }
    }
    public void Init(Vector3 start, Vector3 target, float height, float duration)
    {
        startPos = start;
        targetPos = target;
        this.height = height;
        this.duration = duration;
        t = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Placement"))
        {
            Notify();
            Collider[] _hitedEnemies = Physics.OverlapSphere(transform.position, radius, _enemyMask);
            foreach (var hit in _hitedEnemies)
            {
                var enemy = hit.GetComponent<IDamageable<float>>();

                if (enemy != null)
                {
                    enemy.TakeDamage(200f);
                }
            }
            _anim.SetTrigger("Explode");
        }
    }

    public virtual void Refresh()
    {
        mesh.SetActive(true);
    }

    #region Observable
    public void Subscribe(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Unsubscribe(IObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
        }
    }

    public void Notify()
    {
        foreach (var obs in _observers)
        {
            obs.UpdateData();
        }
    }
    #endregion
}
