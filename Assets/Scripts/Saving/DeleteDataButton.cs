using UnityEngine;
using UnityEngine.UI;

public class DeleteDataButton : MonoBehaviour
{
    Button _deleteDataButton;

    void Awake()
    {
        _deleteDataButton = GetComponent<Button>();
        _deleteDataButton.onClick.AddListener(OnDeleteDataButtonClicked);
    }

    void OnDeleteDataButtonClicked()
    {
        //SaveWithJSON.Instance.DeleteAll();
    }
}