using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DustStorm : MonoBehaviour
{
    [SerializeField] VisualEffect _dustStormFVX;
    [SerializeField] float _speedTransition;
    [SerializeField] float _intensity = 0f;
    [SerializeField] Material _heatScreenMaterial;
    Coroutine _coroutine;
    bool _isActivated;

    void OnEnable()
    {
        EventManager.Subscribe(EventType.OnGameWin, ResetHeatEffect);
        EventManager.Subscribe(EventType.OnGameOver, ResetHeatEffect);
    }

    void Start()
    {
        _dustStormFVX.Stop();
        _heatScreenMaterial.SetFloat("_HeatStrenght", _intensity);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnGameWin, ResetHeatEffect);
        EventManager.Unsubscribe(EventType.OnGameOver, ResetHeatEffect);
    }

    private void ResetHeatEffect(params object[] args)
    {
        _heatScreenMaterial.SetFloat("_HeatStrenght", 0f);
    }

    [ContextMenu("Activate Storm")]
    public void ToggleIsActivated()
    {
        _isActivated = !_isActivated;

        if (_coroutine != null) StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(DustStormRoutine());
    }

    IEnumerator DustStormRoutine()
    {
        if (_isActivated)
        {

            _dustStormFVX.enabled = true;
            _dustStormFVX.Play();

            while (_intensity < 1f)
            {
                _intensity = Mathf.MoveTowards(_intensity, 1f, Time.deltaTime * _speedTransition);
                _heatScreenMaterial.SetFloat("_HeatStrenght", _intensity);
                yield return null;
            }
        }
        else
        {
            _dustStormFVX.Stop();

            while (_intensity > 0f)
            {
                _intensity = Mathf.MoveTowards(_intensity, 0f, Time.deltaTime * _speedTransition);
                _heatScreenMaterial.SetFloat("_HeatStrenght", _intensity);
                yield return null;
            }
        }

        _coroutine = null;
    }
}
