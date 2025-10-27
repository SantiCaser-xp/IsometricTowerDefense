using UnityEngine;

public class SingltonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    [Header("Singleton")]
    [SerializeField] protected bool _doNotDestroyOnLoad;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;

            if (_doNotDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("MonoSingleton: object of type already exists, instance will be destroy = " + typeof(T).Name);
            Destroy(gameObject);
            return;
        }
    }
}