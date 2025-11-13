using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject _winPanel;
    [SerializeField] GameObject _losePanel;

    [SerializeField] private bool _isGameOver;

    private void Start()
    {
        EnemySpawnerLvSystem.AllWavesCleared += OnAllWavesCleared;
    }

    private void OnDestroy()
    {
        EnemySpawnerLvSystem.AllWavesCleared -= OnAllWavesCleared;
    }
    private void OnAllWavesCleared()
    {
        UpdateGameStatus("Win");
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
