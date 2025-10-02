using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldResource : MonoBehaviour, IObservable
{
    [SerializeField] private int _gold = 1;
    [SerializeField] private float _delayToDeactivate = 1;
    private ObjectPool<GoldResource> _pool;
    private CharacterDeposit _characterDeposit;
    private List<IObserver> _observers = new List<IObserver>();
    private MeshRenderer _mesh;
    private Collider[] _colliders;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _colliders = GetComponents<Collider>();
        _mesh = GetComponentInChildren<MeshRenderer>(true);
    }

    public void Initialize(ObjectPool<GoldResource> pool, CharacterDeposit deposit)
    {
        _pool = pool;
        _characterDeposit = deposit;
    }

    public virtual void Refresh()
    { 
        foreach(var col in _colliders)
        {
            col.enabled = true;
        }

        _mesh.enabled = true;
        _rb.isKinematic = false;
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Character>() != null)
        {
            StartCoroutine(DeactivateRoutine());

            foreach (var obs in _observers)
            {
                obs.UpdateData(1);
            }
        }
    }

    private IEnumerator DeactivateRoutine()
    {
        _characterDeposit.AddDeposit(_gold);
        _mesh.enabled = false;
        _rb.isKinematic = true;

        foreach (var col in _colliders)
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(_delayToDeactivate);
        _pool.Release(this);
        yield return null;
    }
}