using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desaturate : MonoBehaviour, IObserver
{
    [SerializeField] Material _material;

    private void Awake()
    {
        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        obs.Subscribe(this);   
    }

    void OnEnable()
    {
        EventManager.Subscribe(EventType.OnGameWin, ResetDesaturation);
        EventManager.Subscribe(EventType.OnGameOver, ResetDesaturation);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnGameWin, ResetDesaturation);
        EventManager.Unsubscribe(EventType.OnGameOver, ResetDesaturation);
    }

    public void UpdateData(params object[] values)
    {
        _material.SetFloat("_Intensity", (float)values[0] / (float)values[1]);
    }

    private void ResetDesaturation(params object[] args)
    {
        _material.SetFloat("_Intensity", 1f);
    }
}