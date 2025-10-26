using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] rewinds;
    public List<IRewind> _rewinds = new List<IRewind>();
    Coroutine _CoroutineRewind;


    public static GameManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else { Destroy(gameObject); }

        foreach (var r in rewinds)
        {
            if (r.GetComponent<IRewind>() != null)
            {
                _rewinds.Add(r.GetComponent<IRewind>());
            }
        }
    }
    private void Start()
    {
        _CoroutineRewind = StartCoroutine(CoroutineSave());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            foreach (var r in _rewinds)
            {
                r.Save();
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (_CoroutineRewind != null)
            {
                StopCoroutine(_CoroutineRewind);
            }
            _CoroutineRewind = StartCoroutine(CoroutineLoad());
        }
    }
    IEnumerator CoroutineSave()
    {
        while (true)
        {
            foreach (var r in _rewinds)
            {
                r.Save();
            }
            yield return null;
        }
    }
    IEnumerator CoroutineLoad()
    {
        bool remember = true;
        while (remember)
        {
            remember = false;
            foreach (var r in _rewinds)
            {
                r.Load();
                if (r.IsRemember())
                {
                    remember = true;
                }
            }
            yield return null;
        }
        _CoroutineRewind = StartCoroutine(CoroutineSave());
    }
}
