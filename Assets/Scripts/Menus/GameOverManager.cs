using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _gameoverScreen;
    [SerializeField] PlayerBase _playerBase;

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.OnGameWin, OnAllWavesCleared);
        EventManager.Subscribe(EventType.OnGameOver, OnBaseDestroyed);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnGameWin, OnAllWavesCleared);
        EventManager.Unsubscribe(EventType.OnGameOver, OnBaseDestroyed);
    }

    private void OnAllWavesCleared(params object[] parameters)
    {
        ScreenManager.Instance.ActivateScreen(_winScreen);
    }

    private void OnBaseDestroyed(params object[] parameters)
    {
        ScreenManager.Instance.ActivateScreen(_gameoverScreen);
    }
}