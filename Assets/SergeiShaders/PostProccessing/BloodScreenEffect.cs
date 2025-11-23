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
        _material.SetFloat("_Alpha", 0f);
    }

    public void UpdateData(params object[] values)
    {
        float current = (float)values[0];

        if (current <= 50) _material.SetFloat("_Alpha", 2f);
    }
}