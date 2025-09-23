using UnityEngine;

public abstract class ControlBase : MonoBehaviour 
{
    protected Vector3 _direction;

    public abstract Vector3 GetDirection();
}