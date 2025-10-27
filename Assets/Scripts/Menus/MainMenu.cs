using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menuBlock;
    [SerializeField] private GameObject background;

    [Header("Settings Buttons")]
    [SerializeField] private Button settingsCloseX;
    [SerializeField] private Button settingsBack;

    void Start()
    {
       // if (settingsButton) settingsButton.onClick.AddListener(OpenSettings);

        //if (settingsCloseX) settingsCloseX.onClick.AddListener(CloseSettings);
        //if (settingsBack) settingsBack.onClick.AddListener(CloseSettings);

        //if (settingsPanel) settingsPanel.SetActive(false);
        if (playButton) playButton.onClick.AddListener(() => SceneTransition.Instance.LoadLevel("WorldMap"));
        //if (exitButton) exitButton.onClick.AddListener(SceneTransition.Instance.ExitFromGame);

        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        if (menuBlock) menuBlock.SetActive(false);
        if (settingsPanel)
        {
            settingsPanel.SetActive(true);
            settingsPanel.transform.SetAsLastSibling();
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel) settingsPanel.SetActive(false);
        if (menuBlock) menuBlock.SetActive(true);
    }
}