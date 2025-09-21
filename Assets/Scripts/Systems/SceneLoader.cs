using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    { Time.timeScale = 1f; SceneManager.LoadScene(sceneName); }

    public void LoadMainMenu()
    { Time.timeScale = 1f; SceneManager.LoadScene("SampleScene"); }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
