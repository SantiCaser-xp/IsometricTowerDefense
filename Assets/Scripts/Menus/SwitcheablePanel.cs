using UnityEngine;

public class SwitcheablePanel : MonoBehaviour
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
