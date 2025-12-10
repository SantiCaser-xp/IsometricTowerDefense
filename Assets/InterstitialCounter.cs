using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterstitialCounter : MonoBehaviour
{
    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        EventManager.Trigger(EventType.ButtonForInterstitialClicked);
    }
}
