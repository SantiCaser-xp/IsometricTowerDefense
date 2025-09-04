using System;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour, IResettable<float>
{
    public static Action OnLevelUp;

    [Header("Experience")]
    [SerializeField] private float _maxExperienceLimit = 9999f;
    [SerializeField] private float _startExperienceThreshold = 10f;
    [SerializeField] private float _experienceGainMultiplier = 1.2f;
    private float _currentExperience = 0f;
    private float _currentExperienceThreshold = 0f;

    private void Awake()
    {
        _currentExperienceThreshold = _startExperienceThreshold;
        //initialize the variables for UI
    }

    public void AddExperience(float value)
    {
        if (value <= 0f) return;

        _currentExperience += value;

        _currentExperience = Mathf.Clamp(_currentExperience, 0f, _maxExperienceLimit);

        if(_currentExperience >= _currentExperienceThreshold)
        {
            _currentExperience -= _currentExperienceThreshold;
            RecalculateExperienceThreshold();
            OnLevelUp?.Invoke();
        }
    }

    private void RecalculateExperienceThreshold()
    {
        _currentExperienceThreshold *= _experienceGainMultiplier;
    }

    public void Reset()
    {
        _currentExperienceThreshold = _startExperienceThreshold;
        _currentExperience = 0f;
    }
}