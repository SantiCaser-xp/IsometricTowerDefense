using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField,Range(0.35f,1f)] float _effectPower;
    [SerializeField] float _workInterval;
    [SerializeField] float _speed;
    MeshRenderer _meshRenderer;
    Collider _collider;
    Coroutine _coroutine;
    bool _isActivated;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    public void ToggleIsActivated()
    {
        _isActivated = !_isActivated;

        if (_coroutine != null) StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShieldRoutine());
    }

    IEnumerator ShieldRoutine()
    {
        if(_isActivated)
        {
            _collider.enabled = true;
            
            while (_effectPower < 1f)
            {
                _effectPower = Mathf.MoveTowards(_effectPower, 1f, Time.deltaTime * _speed);
                _meshRenderer.material.SetFloat("_CutEdge", _effectPower);
                yield return null;
            }
            Debug.Log("Shield fully active. Waiting...");
            yield return new WaitForSeconds(_workInterval);
        }


            while (_effectPower > 0.36f)
            {
                _effectPower = Mathf.MoveTowards(_effectPower, 0.35f, Time.deltaTime * _speed);
                _meshRenderer.material.SetFloat("_CutEdge", _effectPower);
                yield return null;
            }

            _collider.enabled = false;
        

        _coroutine = null;
    }
}
