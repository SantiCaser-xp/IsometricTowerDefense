using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject _winPanel;
    [SerializeField] GameObject _losePanel;
    [SerializeField] private PlayerBase playerBase;

    [SerializeField] private bool _isGameOver;

    private void Start()
    {
        EnemySpawnerLvSystem.AllWavesCleared += OnAllWavesCleared;
        if (playerBase != null)
        {
            playerBase.OnPlayerBaseDestroyed += OnBaseDestroyed;
        }
    }

    private void OnDestroy()
    {
        EnemySpawnerLvSystem.AllWavesCleared -= OnAllWavesCleared;
        playerBase.OnPlayerBaseDestroyed -= OnBaseDestroyed;
    }
    private void OnAllWavesCleared()
    {
        UpdateGameStatus("Win");
    }

    private void OnBaseDestroyed()
    {
        UpdateGameStatus("Lose");
    }
    public void UpdateGameStatus(string status)
    {
        Debug.Log("GameOverSystem: UpdateGameStatus called with status: " + status);
        if (status == "Win")
        {
            Debug.Log("GameOverSystem: Win detected");
            if (_isGameOver) return;
            _isGameOver = true;

            Time.timeScale = 0f;

            if (_winPanel)
            {
                _winPanel.SetActive(true);
            }
        }
        else if (status == "Lose")
        {
            if (_isGameOver) return;
            _isGameOver = true;

            Time.timeScale = 0f;

            if (_losePanel)
            {
                _losePanel.SetActive(true);
            }
        }
    }
}
