using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFactory<T> : MonoBehaviour
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _initialStock;
    protected ObjectPool<T> _pool;

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>(CreatePrefab, TurnOff, TurnOn, _initialStock);
    }

    protected abstract void TurnOn(T obj);
    protected abstract void TurnOff(T obj);
    protected abstract T CreatePrefab();
    public abstract T Create();
}
