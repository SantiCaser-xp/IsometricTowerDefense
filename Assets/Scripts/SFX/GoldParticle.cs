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

    public void UpdateData(params object[] values)
    {
        //_particleSystem.Play();
        _goldFVX.Play();
    }
}