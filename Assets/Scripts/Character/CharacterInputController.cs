using UnityEngine;

public class CharacterInputController
{
    private Vector3 _inputDirection = new Vector3();
    public Vector3 InputDirection => _inputDirection;

    private ControlBase _joystick;

    public CharacterInputController(ControlBase joystick)
    {
        _joystick = joystick;
    }

    public void InputArtificialUpdate()
    {

#if UNITY_ANDROID
        InputAndroid();
#else
        InputPC();
        _joystick.transform.parent.gameObject.SetActive(false);
#endif
    }

    public void InputPC()
    {
        _inputDirection.x = Input.GetAxisRaw("Horizontal");
        _inputDirection.z = Input.GetAxisRaw("Vertical");
    }

    public void InputAndroid()
    {
        _inputDirection = _joystick.GetDirection();   
    }
}