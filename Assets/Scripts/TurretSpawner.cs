using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private SimpleTower _simpleTower;

    private void Start()
    {
        _collider.isTrigger = true;
        _simpleTower.enabled = false;

    }

    public void StartPlacement()
    {
        _collider.isTrigger = true;
        _simpleTower.enabled = false;
    }

    public void StopPlacement()
    {
        _collider.isTrigger = false;
        _simpleTower.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            return;
        }
        else
        {
            StartPlacement();
        }
    }

   
}
