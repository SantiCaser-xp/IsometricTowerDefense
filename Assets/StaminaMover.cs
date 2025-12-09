using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaMover : MonoBehaviour
{
    private void Start()
    {
        EventManager.Subscribe(EventType.OnBannerAdShown, AdAppear);
        EventManager.Subscribe(EventType.OnBannerAdDisable, AdDissapear);
    }

    private void AdAppear(params object[] parameters)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Ejemplo: mueve la UI a la posición (x: 200, y: -100)
            rectTransform.anchoredPosition = new Vector2(0, 180);
        }
    }

    private void AdDissapear(params object[] parameters)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Ejemplo: mueve la UI a la posición (x: 200, y: -100)
            rectTransform.anchoredPosition = new Vector2(0, 25);
        }
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnBannerAdShown, AdAppear);
        EventManager.Unsubscribe(EventType.OnBannerAdDisable, AdDissapear);
    }
}
