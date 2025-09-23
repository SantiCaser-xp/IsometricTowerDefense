using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static Action<int> OnLevelChanged;

    [Header("Level")]
    [SerializeField] private int _maxLevel = 100;
    private int _startLevel = 1;
    private int _currentLevel = 0;

    private void Awake()
    {
        _currentLevel = _startLevel;
    }

    private void AddLevel()
    {
        if (_currentLevel < _maxLevel)
        {
            _currentLevel++;
            OnLevelChanged?.Invoke(_currentLevel);
        }
    }
}