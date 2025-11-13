using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public void NextLevel()
    {
        Time.timeScale = 1f; // Resume normal time scale
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f; // Resume normal time scale
        SceneManager.LoadScene("WorldMap");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Resume normal time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}