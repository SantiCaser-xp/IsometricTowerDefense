using UnityEngine;

public class BloodScreenEffect : MonoBehaviour, IObserver
{
    [SerializeField] Material _material;

    private void Awake()
    {
        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        obs.Subscribe(this);
        ResetBloodScreen();
    }

    void OnEnable()
    {
        EventManager.Subscribe(EventType.OnGameWin, ResetBloodScreen);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnGameWin, ResetBloodScreen);
    }

    public void UpdateData(params object[] values)
    {
        float current = (float)values[0];

        if (current <= 50) _material.SetFloat("_Intensity", 1f);
    }

    void ResetBloodScreen(params object[] arg)
    {
        _material.SetFloat("_Intensity", 0f);
    }
}