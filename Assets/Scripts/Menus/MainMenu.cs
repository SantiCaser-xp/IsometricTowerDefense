using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;

    void Start()
    {
        playButton.onClick.AddListener(() => SceneTransition.Instance.LoadLevel("WorldMap"));
    }
}