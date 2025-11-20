using UnityEngine;

public class TimerDay : MonoBehaviour
{
    [SerializeField] float _delayStart = 3f;  
    [SerializeField] float _interval = 5f;    
    [SerializeField] Rain _rain;
    int _wheaterIndex;
    float _nextTime;

    void Start()
    {
        _nextTime = Time.time + _delayStart;
    }

    void Update()
    {
        if (Time.time >= _nextTime)
        {
            _rain.ToggleIsActivated();

            _nextTime = Time.time + _interval;
        }
    }
}