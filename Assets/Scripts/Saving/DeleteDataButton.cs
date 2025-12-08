using UnityEngine;
using UnityEngine.UI;

public class DeleteDataButton : MonoBehaviour
{
    [SerializeField] GameObject _confirmPanel;
    Button _deleteDataButton;

    void Awake()
    {
        _deleteDataButton = GetComponent<Button>();
    }

    void Start()
    {
        _deleteDataButton.onClick.AddListener(() => ScreenManager.Instance.ActivateScreen(_confirmPanel));
    }
}