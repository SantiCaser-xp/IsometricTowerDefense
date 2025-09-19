using UnityEngine;
using UnityEngine.UI;

public class TowerHealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_mainCamera != null)
        {
            transform.LookAt(transform.position + _mainCamera.transform.forward);
        }
    }

    public void SetHealth(float current, float max)
    {
        _fillImage.fillAmount = current / max;
    }
}
