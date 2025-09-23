using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] protected Transform[] _spawnPoints;
   // [SerializeField] protected T[] _prefabs;
    [SerializeField] protected float _secondToSpawn = 1f;
    [SerializeField] protected T _factory;

    protected virtual void Start()
    {
        InvokeRepeating(nameof(Spawn), 0, _secondToSpawn);
    }
    protected virtual void Spawn()
    {     
        
    }  
    
   
}
