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

    public void UpdateData(params object[] values)
    {
        float currentValue = (float)values[0];
        float maxValue = (float)values[1];

        _fillImage.fillAmount = currentValue / maxValue;
    }
}