using UnityEngine;

public class MobileBackHandler : MonoBehaviour
{
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] bool isMainMenuScene = true;

    float _lastBack; const float WINDOW = 1.2f;

    void Update()
    {
        // Android Back и Esc в Editor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsCanvas && settingsCanvas.activeSelf) { settingsCanvas.SetActive(false); return; }
            float t = Time.unscaledTime;
            if (t - _lastBack < WINDOW) sceneLoader?.QuitGame();
            else { _lastBack = t; Debug.Log("Press Back again to exit"); }
        }
    }
}
