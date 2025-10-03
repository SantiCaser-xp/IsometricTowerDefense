using UnityEngine;

public class WinLoseSFX : MonoBehaviour, IObserver
{
    [SerializeField] private AudioClip _clipWin;
    [SerializeField] private AudioClip _clipLose;
    private AudioSource _audiosource;

    private void Awake()
    {
        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        obs.Subscribe(this);

        _audiosource = GetComponent<AudioSource>();
        //_audiosource.ignoreListenerPause = true;
    }

    public void UpdateGameStatus(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.Win:
                _audiosource.PlayOneShot(_clipWin);
                break;
            case GameStatus.Lose:
                _audiosource.PlayOneShot(_clipLose);
                break;
        }
    }

    public void UpdateData(float currentValue, float maxValue) { }
    public void UpdateData(int value) { }
}