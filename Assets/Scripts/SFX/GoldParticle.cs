using UnityEngine;
using UnityEngine.VFX;

public class GoldParticle : MonoBehaviour, IObserver
{
    //[SerializeField] ParticleSystem _particleSystem;
    [SerializeField] VisualEffect _goldFVX;

    private void Awake()
    {
        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        obs.Subscribe(this);

        _goldFVX.Stop();
    }

    public void UpdateData(float currentValue, float maxValue) { }

    public void UpdateData(int value)
    {
        //_particleSystem.Play();
        _goldFVX.Play();
    }

    public void UpdateGameStatus(GameStatus status) { }
}