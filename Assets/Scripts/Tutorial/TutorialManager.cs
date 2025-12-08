using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI _tutorialText;

    [Header("Steps")]
    [SerializeField] List<TutorialStep> _steps = new List<TutorialStep>();

    int _currentStepIndex = 0;

    void Start()
    {
        ShowCurrentStep();
    }

    void ShowCurrentStep()
    {
        if (_currentStepIndex >= _steps.Count)
        {
            _tutorialText.text = "";
            return;
        }

        _tutorialText.text = _steps[_currentStepIndex].text;
    }

    void OnEnable()
    {
        EventManager.Subscribe(EventType.MoveJoystick, OnEventReceived);
        EventManager.Subscribe(EventType.ZoomCamera, OnEventReceived);
        EventManager.Subscribe(EventType.OpenBuildMenu, OnEventReceived);
        EventManager.Subscribe(EventType.PlaceBuilding, OnEventReceived);
        EventManager.Subscribe(EventType.KillFirstEnemy, OnEventReceived);
        EventManager.Subscribe(EventType.FirstGoldCatched, OnEventReceived);
        EventManager.Subscribe(EventType.ActivateShield, OnEventReceived);
        EventManager.Subscribe(EventType.Survive, OnEventReceived);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(EventType.MoveJoystick, OnEventReceived);
        EventManager.Unsubscribe(EventType.ZoomCamera, OnEventReceived);
        EventManager.Unsubscribe(EventType.OpenBuildMenu, OnEventReceived);
        EventManager.Unsubscribe(EventType.PlaceBuilding, OnEventReceived);
        EventManager.Unsubscribe(EventType.KillFirstEnemy, OnEventReceived);
        EventManager.Unsubscribe(EventType.FirstGoldCatched, OnEventReceived);
        EventManager.Unsubscribe(EventType.ActivateShield, OnEventReceived);
        EventManager.Unsubscribe(EventType.Survive, OnEventReceived);
    }

    void OnEventReceived(params object[] args)
    {
        if (_currentStepIndex >= _steps.Count)
            return;

        EventType receivedEvent = (EventType)args[0];

        if (_steps[_currentStepIndex].completeEvent == receivedEvent)
        {
            _currentStepIndex++;
            ShowCurrentStep();
        }
    }
}