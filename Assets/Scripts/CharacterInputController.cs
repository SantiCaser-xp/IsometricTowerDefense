using UnityEngine;

public class CharacterInputController
{
    private Vector3 _inputDirection = new Vector3();
    public Vector3 InputDirection => _inputDirection;

    private ControlBase _joystick;
    private bool _useJoystick;

    public CharacterInputController(ControlBase joystick)
    {
        _joystick = joystick;

#if UNITY_ANDROID
        _useJoystick = true;
#else
        _useJoystick = false;
#endif
    }

    public void InputArtificialUpdate()
    {
        if (_useJoystick && _joystick != null)
        {
            InputAndroid();
        }
        else
        {
            InputPC();
        }
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