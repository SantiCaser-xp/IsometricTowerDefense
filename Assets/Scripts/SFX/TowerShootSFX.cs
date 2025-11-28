using UnityEngine;

public class TowerShootSFX : MonoBehaviour, IObserver
{
    private AudioSource _audiosource;
    [SerializeField] private AudioClip _clip;

    private void Awake()
    {
        _audiosource = GetComponent<AudioSource>();

        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        obs.Subscribe(this);
    }

    public void UpdateData(params object[] values)
    {
        _audiosource.PlayOneShot(_clip);
    }

    public void StopSFX()
    {
        _audiosource.Stop();
    }
}