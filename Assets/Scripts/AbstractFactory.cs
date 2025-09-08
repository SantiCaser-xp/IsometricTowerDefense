using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFactory<T> : MonoBehaviour
{
    [SerializeField] protected T _prefab;
    public abstract T Create();
}
