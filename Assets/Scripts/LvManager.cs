using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LvManager : MonoBehaviour
{
    public PlayerBase playerBase;

    public static LvManager Instance;

    public int enemiesKilled;
    [SerializeField] private int enemiesToKill;

    [SerializeField] private TextMeshProUGUI enemiesToKillText;

    public event System.Action OnGameOver;
    public event System.Action OnWin;
    void Start()
    {
        //Instance = this;
        enemiesToKillText.text = $"Enemies Killed: {enemiesKilled} / {enemiesToKill}";

        if (LvManager.Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (playerBase != null)
        {
            playerBase.OnPlayerBaseDestroyed += OnPlayerBaseDiedHandler;
        }

        BaseEnemy.OnEnemyKilled += EnemyKilled;
    }

    private void OnDestroy()
    {
        BaseEnemy.OnEnemyKilled -= EnemyKilled;
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        enemiesToKillText.text = $"Enemies Killed: {enemiesKilled} / {enemiesToKill}";
        if (enemiesKilled >= enemiesToKill)
        {
            Debug.Log("All Enemies Killed! You Win!");
            OnWin?.Invoke();
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
