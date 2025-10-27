using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : SingltonBase<LevelManager>
{
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void RestartLevel()
    {
        SceneManager.GetActiveScene();
    }

    public void ExitFromGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}