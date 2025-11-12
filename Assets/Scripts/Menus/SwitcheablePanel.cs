using UnityEngine;

public class SwitcheablePanel : MonoBehaviour
{
    private void Start()
    {
        TogglePanel();
    }

    public void TogglePanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
