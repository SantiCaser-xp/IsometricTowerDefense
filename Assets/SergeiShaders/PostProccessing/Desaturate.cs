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

    void Start()
    {
        _material.SetFloat("_Rang", 1f);
    }

    public void UpdateData(params object[] values)
    {
        _material.SetFloat("_Rang", (float)values[0] / (float)values[1]);
    }
}