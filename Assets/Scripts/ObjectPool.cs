using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    public delegate T FactoryMethod();

    FactoryMethod _factory;
    Action<T> _TurnOff;
    Action<T> _TurnOn;
    List<T> _stockAvailables = new List<T>();

    public ObjectPool(FactoryMethod Factory, Action<T> TurnOff, Action<T> TurnOn, int initialStock = 5)
    {
        _factory = Factory;
        _TurnOff = TurnOff;
        _TurnOn = TurnOn;

        for (int i = 0; i < initialStock; i++)
        {
            var x = _factory();

            _TurnOff(x);
            _stockAvailables.Add(x);
        }
    }

    public T Get()
    {
        if (_stockAvailables.Count > 0)
        {
            var x = _stockAvailables[0];
            _stockAvailables.RemoveAt(0);

            _TurnOn(x);
            return x;
        }

        return _factory();
    }

    public void Release(T obj)
    {
        Debug.Log("Releasing object back to pool: " + obj.ToString());
        _TurnOff(obj);
        _stockAvailables.Add(obj);
    }
}
