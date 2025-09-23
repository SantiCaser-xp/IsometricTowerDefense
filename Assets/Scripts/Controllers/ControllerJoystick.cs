using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerJoystick : ControlBase, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _maxMagnitude = 10f;
    private Vector3 _initialPos;

    private void Start()
    {
        _initialPos = transform.position;
    }

    public override Vector3 GetDirection()
    {
        Vector3 modifiedDir = new Vector3(_direction.x, 0, _direction.y);

        modifiedDir /= _maxMagnitude;
        return modifiedDir;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _direction = Vector3.ClampMagnitude((Vector3)eventData.position - _initialPos, _maxMagnitude);

        transform.position = _initialPos + _direction;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _initialPos;
        _direction = Vector3.zero;
    }
}