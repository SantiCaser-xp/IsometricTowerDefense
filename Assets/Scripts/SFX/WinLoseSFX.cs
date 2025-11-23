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

    public void UpdateData(params object[] values)
    {
        GameStatus status = (GameStatus)values[0];

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
}