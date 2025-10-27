using UnityEngine;

public class SingltonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    [Header("Singleton")]
    [SerializeField] private bool m_DoNotDestroyOnLoad;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("MonoSingleton: object of type already exists, instance will be destroy = " + typeof(T).Name);
            Destroy(gameObject);
            return;
        }

        Instance = this as T;

        if (m_DoNotDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}
