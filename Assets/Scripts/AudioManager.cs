using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = true;
    }
    #endregion

    [Header("Mixer & Params")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _masterFloatName = "MasterVolume";
    [SerializeField] private string _musicFloatName = "MusicVolume";
    [SerializeField] private string _sfxFloatName = "SFXVolume";
    [SerializeField] private string _uiFloatName = "UIVolume";

    [Header("Defaults 0..1")]
    [SerializeField] private float _initMasterVol = 0.75f;
    [SerializeField] private float _initMusicVol = 0.5f;
    [SerializeField] private float _initSFXVol = 0.5f;
    [SerializeField] private float _initUIVol = 0.2f;

    public float InitMasterVol => _initMasterVol;
    public float InitMusicVol => _initMusicVol;
    public float InitSFXVol => _initSFXVol;
    public float InitUIVol => _initUIVol;

    private float _curMaster, _curMusic, _curSFX, _curUI;

    private AudioSource _audioSource;

    private void Start()
    {
        SetMasterVolume(_initMasterVol);
        SetMusicVolume(_initMusicVol);
        SetSFXVolume(_initSFXVol);
        SetUIVolume(_initUIVol);
    }

    public void SetMasterVolume(float value)
    {
        _curMaster = Clamp01(value);
        SetMixer01(_masterFloatName, _curMaster);
    }

    public void SetMusicVolume(float value)
    {
        _curMusic = Clamp01(value);
        SetMixer01(_musicFloatName, _curMusic);
    }

    public void SetSFXVolume(float value)
    {
        _curSFX = Clamp01(value);
        SetMixer01(_sfxFloatName, _curSFX);
    }

    public void SetUIVolume(float value)
    {
        _curUI = Clamp01(value);
        SetMixer01(_uiFloatName, _curUI);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (!clip) return;

        if (_audioSource.isPlaying && _audioSource.clip == clip)
            return;

        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void PlayUI(AudioClip clip)
    {
        if (clip) _audioSource.PlayOneShot(clip);
    }

    private bool _nonMusicMuted = false;
    private float _prevSFX = 0.5f;
    private float _prevUI = 0.2f;


    public void MuteNonMusicDuringPause(bool mute)
    {
        if (mute)
        {
            if (_nonMusicMuted) return;
            _nonMusicMuted = true;

            _prevSFX = _curSFX;
            _prevUI = _curUI;

            SetMixer01_NoSave(_sfxFloatName, 0.0001f);
            SetMixer01_NoSave(_uiFloatName, 0.0001f);
        }
        else
        {
            if (!_nonMusicMuted) return;
            _nonMusicMuted = false;

            SetMixer01_NoSave(_sfxFloatName, _prevSFX);
            SetMixer01_NoSave(_uiFloatName, _prevUI);
        }
    }

    private static float Clamp01(float v) => (v <= 0f) ? 0.0001f : Mathf.Clamp01(v);

    private void SetMixer01(string exposedName, float v01)
    {
        float linear = Clamp01(v01);
        _audioMixer.SetFloat(exposedName, Mathf.Log(linear) * 20f);
    }

    private void SetMixer01_NoSave(string exposedName, float v01)
    {
        float linear = Clamp01(v01);
        _audioMixer.SetFloat(exposedName, Mathf.Log(linear) * 20f);
    }
}
