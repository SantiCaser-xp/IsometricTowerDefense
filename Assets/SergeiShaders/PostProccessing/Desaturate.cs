using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desaturate : MonoBehaviour,IObserver
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

    public void UpdateData(float currentHealth, float maxHealth)
    {
        _material.SetFloat("_Rang", currentHealth / maxHealth);
    }
    public void UpdateDataTime(float currentHealth, float maxHealth) { }

    public void UpdateData(int value) { }

    public void UpdateGameStatus(GameStatus status) { }

    public void UpdateData(params object[] values)
    {
        _material.SetFloat("_Rang", (float)values[0] / (float)values[1]);
    }
}