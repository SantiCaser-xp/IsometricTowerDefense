using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasting : MonoBehaviour
{
    [SerializeField] private float _rayDistance = 10f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private Transform _rayOrigin;
    private Ray _ray;
    private RaycastHit _hitInfo;
    private bool _isHit = false;


    private void OnDrawGizmos()
    {
        _ray = new Ray(_rayOrigin.position, _ray.direction * _rayDistance);

        Gizmos.color = _isHit ? Color.red : Color.green;
        Gizmos.DrawRay(_rayOrigin.position, _ray.direction * _rayDistance);
    }

    private void Update()
    {
        _spawnPos.position = _rayOrigin.position + _rayOrigin.forward * _rayDistance;

        IsOverlaping();
    }


    private void Start()
    {
    }

    public bool IsOverlaping()
    {
        _ray = new Ray(_rayOrigin.position, _rayOrigin.forward);
        _isHit = Physics.Raycast(_ray, out _hitInfo, _rayDistance, _layerMask);
        return _isHit;
    }





}
