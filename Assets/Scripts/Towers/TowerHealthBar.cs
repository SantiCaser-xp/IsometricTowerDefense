using UnityEngine;
using UnityEngine.UI;

public class TowerHealthBar : MonoBehaviour, IObserver
{
    [SerializeField] private Image _fillImage;

    private Camera _mainCamera;

    private void Awake()
    {
        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        obs.Subscribe(this);

        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_mainCamera != null)
        {
            transform.LookAt(transform.position + -_mainCamera.transform.forward);
        }
    }

    public void UpdateData(float currentValue, float maxValue)
    {
        _fillImage.fillAmount = currentValue / maxValue;

        if (currentValue <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateData(int value) { }
}
