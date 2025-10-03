using UnityEngine;

public class GoldSFX : MonoBehaviour, IObserver
{
    [SerializeField] private AudioSource _audiosource;
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

    public void UpdateData(float currentValue, float maxValue) { }

    public void UpdateData(int value)
    {
        _audiosource.PlayOneShot(_clip);
    }

    public void UpdateGameStatus(GameStatus status) { }
}