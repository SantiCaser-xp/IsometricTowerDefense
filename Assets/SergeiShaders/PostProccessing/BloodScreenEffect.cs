using UnityEngine;
using UnityEngine.SceneManagement;

public class BloodScreenEffect : MonoBehaviour, IObserver
{
    [SerializeField] Material _material;

    private void Awake()
    {
        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            _material.SetFloat("_Alpha", 0f);
        };

        obs.Subscribe(this);
        _material.SetFloat("_Alpha", 0f);

    }

    private void Start()
    {
        //UpdateData(100f);
    }



    public void UpdateData(params object[] values)
    {
        Debug.Log("Blood Screen Effect UpdateData called with value: " + values[0]);
        float current = (float)values[0];

        if (current <= 50) _material.SetFloat("_Alpha", 2f);
    }
}