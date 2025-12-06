using UnityEngine;

public class ZoomTouch : MonoBehaviour
{
    Camera _camera;
    [SerializeField] float _zoomStep = 3f;
    [SerializeField] float _zoomMin = 20f;
    [SerializeField] float _zoomMax = 90f;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if(Input.touchCount != 2) return;

        Touch first = Input.touches[0];
        Touch second = Input.touches[1];

        Vector2 firstTouchLastPos = first.position - first.deltaPosition;
        Vector2 secondTouchLastPos = second.position - second.deltaPosition;

        Vector2 magnitudeBetweenTouches = first.position - second.position;

        Vector2 magnitudeBetweenDelta = firstTouchLastPos - secondTouchLastPos;

        float zoom = magnitudeBetweenTouches.magnitude - magnitudeBetweenDelta.magnitude;

        if(zoom != 0)
        {
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
}
