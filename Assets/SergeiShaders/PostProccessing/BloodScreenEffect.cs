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

    public void UpdateData(float currentHealth, float maxHealth)
    {
        if(currentHealth <= 50) _material.SetFloat("_Alpha", 2f);
    }

    public void UpdateData(int value) { }

    public void UpdateGameStatus(GameStatus status) { }

    public void UpdateDataTime(float currentHealth, float maxHealth) { }

    public void UpdateData(params object[] values)
    {
        float current = (float)values[0];

        if (current <= 50) _material.SetFloat("_Alpha", 2f);
    }
}