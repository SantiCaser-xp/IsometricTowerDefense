using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvManager : MonoBehaviour
{
    public PlayerBase playerBase;

    public event System.Action OnGameOver;
    void Start()
    {
        if (playerBase != null)
        {
            playerBase.OnPlayerBaseDestroyed += OnPlayerBaseDiedHandler;
        }
    }

    private void OnPlayerBaseDiedHandler()
    {
        Debug.Log("Player Base Destroyed! Game Over!");
        OnGameOver?.Invoke();
    }

    private void OnDisable()
    {
        if (playerBase != null)
        {
            playerBase.OnPlayerBaseDestroyed -= OnPlayerBaseDiedHandler;
        }
    }
}
