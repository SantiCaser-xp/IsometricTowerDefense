using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DustStorm : MonoBehaviour
{
    [SerializeField] VisualEffect _dustStormFVX;
    [SerializeField] float _speedTransition;
    [SerializeField] float _intensity = 0f;
    [SerializeField] Material _heatScreenMaterial;
    [SerializeField] Material _dirtScreenMaterial;
    AudioSource _audioSource;
    Coroutine _coroutine;
    bool _isActivated;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _dustStormFVX.Stop();
        _heatScreenMaterial.SetFloat("_HeatStrenght", _intensity);
        _dirtScreenMaterial.SetFloat("_AlphaStrenght", _intensity);
    }

    [ContextMenu("Activate Storm")]
    public void ToggleIsActivated()
    {
        _isActivated = !_isActivated;

        if (_coroutine != null) StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShieldRoutine());
    }

    IEnumerator ShieldRoutine()
    {
        _audioSource.Play();

        if (_isActivated)
        {

            _dustStormFVX.enabled = true;
            _dustStormFVX.Play();

            while (_intensity < 1f)
            {
                _intensity = Mathf.MoveTowards(_intensity, 1f, Time.deltaTime * _speedTransition);
                _heatScreenMaterial.SetFloat("_HeatStrenght", _intensity);
                _dirtScreenMaterial.SetFloat("_AlphaStrenght", _intensity);
                yield return null;
            }
        }
        else
        {
            _dustStormFVX.Stop();
            _audioSource.Stop();
            while (_intensity > 0f)
            {
                _intensity = Mathf.MoveTowards(_intensity, 0f, Time.deltaTime * _speedTransition);
                _heatScreenMaterial.SetFloat("_HeatStrenght", _intensity);
                _dirtScreenMaterial.SetFloat("_AlphaStrenght", _intensity);
                yield return null;
            }
        }

        _coroutine = null;
    }
}
