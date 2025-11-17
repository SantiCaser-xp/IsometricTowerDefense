using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Rain : MonoBehaviour//доделать вызов дождя и настройки плавного появления луж и вызов шейдера скрина
{
    [SerializeField] float _effectPower = 1f;
    [SerializeField] float _effectScreenPower = 0f;
    [SerializeField] float _speed;
    [SerializeField] float _speedScreen;
    [SerializeField] VisualEffect _rainVFX;
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] Material _material;
    Coroutine _coroutine;
    bool _isActivated;

    private void Start()
    {
        _rainVFX.Stop();
        _meshRenderer.material.SetFloat("_PoundIntencity", _effectPower);
        _material.SetFloat("_Intensity", _effectScreenPower);
    }

    [ContextMenu("Activate Rain")]
    public void ToggleIsActivated()
    {
        _isActivated = !_isActivated;

        if (_coroutine != null) StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShieldRoutine());
    }

    IEnumerator ShieldRoutine()
    {
        if (_isActivated)
        {
            
            _rainVFX.enabled = true;
            _rainVFX.Play();

            while (_effectPower > 0.5f)
            {
                _effectPower = Mathf.MoveTowards(_effectPower, 0.5f, Time.deltaTime * _speed);
                _effectScreenPower = Mathf.MoveTowards(_effectScreenPower, 1f, Time.deltaTime * _speedScreen);
                _meshRenderer.material.SetFloat("_PoundIntencity", _effectPower);
                _material.SetFloat("_Intensity", _effectScreenPower);
                yield return null;
            }
        }
        else
        {
            _rainVFX.Stop();

            while (_effectPower < 1f)
            {
                _effectPower = Mathf.MoveTowards(_effectPower, 1f, Time.deltaTime * _speed);
                _effectScreenPower = Mathf.MoveTowards(_effectScreenPower, 0f, Time.deltaTime * _speedScreen);
                _meshRenderer.material.SetFloat("_PoundIntencity", _effectPower);
                _material.SetFloat("_Intensity", _effectScreenPower);
                yield return null;
            }
        }

        _coroutine = null;
    }
}
