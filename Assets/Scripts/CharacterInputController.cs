using UnityEngine;

public class CharacterInputController
{
    private Vector3 _inputDirection = new Vector3();
    public Vector3 InputDirection => _inputDirection;

    public void InputArtificialUpdate()
    {
        InputPC();
    }

    public void InputPC()
    {
        _inputDirection.x = Input.GetAxisRaw("Horizontal");
        _inputDirection.z = Input.GetAxisRaw("Vertical");
    }
}