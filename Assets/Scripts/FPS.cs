using UnityEngine;

public class FPS : MonoBehaviour
{
    float smoothedFPS;

    private void Start()
    {  
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        float currentFPS = 1f / Time.unscaledDeltaTime;
        smoothedFPS = Mathf.Lerp(smoothedFPS, currentFPS, 0.1f);
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 32;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(500, 10, 300, 100), "FPS: " + Mathf.RoundToInt(smoothedFPS), style);
    }
}