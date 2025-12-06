using UnityEngine;

public class ZoomTouch : MonoBehaviour
{
    Camera _camera;
    [Header("Mobile")]
    [SerializeField] float _zoomStep = 3f;
    [SerializeField] float _zoomMin = 20f;
    [SerializeField] float _zoomMax = 90f;

    [Header("PC")]
    [SerializeField] float _zoomWheelStep = 200f;

    [SerializeField] bool _tutorialMode = false;
    float _scroll;


    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll == 0f)
            return;

        if (_tutorialMode)
        {
            EventManager.Trigger(EventType.ZoomCamera, EventType.ZoomCamera);
        }

        if (_camera.orthographic)
        {
            _camera.orthographicSize = Mathf.Clamp(
                _camera.orthographicSize - scroll * _zoomWheelStep * Time.deltaTime,
                _zoomMin,
                _zoomMax
            );
        }
        else
        {
            _camera.fieldOfView = Mathf.Clamp(
                _camera.fieldOfView - scroll * _zoomWheelStep * Time.deltaTime,
                _zoomMin,
                _zoomMax
            );
        }

#else


        if (Input.touchCount != 2) return;

        Touch first = Input.touches[0];
        Touch second = Input.touches[1];

        Vector2 firstTouchLastPos = first.position - first.deltaPosition;
        Vector2 secondTouchLastPos = second.position - second.deltaPosition;

        Vector2 magnitudeBetweenTouches = first.position - second.position;

        Vector2 magnitudeBetweenDelta = firstTouchLastPos - secondTouchLastPos;

        float zoom = magnitudeBetweenTouches.magnitude - magnitudeBetweenDelta.magnitude;

        if(zoom != 0)
        {
            if (_tutorialMode)
            {
                EventManager.Trigger(EventType.ZoomCamera, EventType.ZoomCamera);
            }

            if (_camera.orthographic)
            {
                _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + CalculateZoom(zoom), _zoomMin, _zoomMax);
            }
            else
            {
                _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView + CalculateZoom(zoom), _zoomMin, _zoomMax);
            }
        }

        float CalculateZoom(float zoom)
        {

            return zoom * _zoomStep * Time.deltaTime;
        }
        
    }
#endif
    }
}