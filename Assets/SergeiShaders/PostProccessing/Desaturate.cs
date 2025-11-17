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

    public void UpdateData(int value) { }

    public void UpdateGameStatus(GameStatus status) { }
}
