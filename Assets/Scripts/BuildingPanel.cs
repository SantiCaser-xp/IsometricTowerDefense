using UnityEngine;

public class BuildingPanel : MonoBehaviour
{
    private void Awake()
    {
        TogglePanel();
    }

    public void TogglePanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
