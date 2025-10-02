using UnityEngine;

public class GoldParticle : MonoBehaviour, IObserver 
{
    [SerializeField] ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        obs.Subscribe(this);
    }

    public void UpdateData(float currentValue, float maxValue)
    {

    }

    public void UpdateData(int value)
    {
        _particleSystem.Play();
    }
}