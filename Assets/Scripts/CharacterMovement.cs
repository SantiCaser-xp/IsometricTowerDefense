using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 3.5f;
    [SerializeField] private float _movementSpeedMultiplier = 1f;
    [SerializeField] private IncrementVelocityPerk _incrementVelocityPerk;
    private Rigidbody _rb;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Movement(Vector3 dir)
    {
        _moveDirection = (transform.right * dir.x + transform.forward * dir.z);
        _rb.MovePosition(_rb.position + _moveDirection.normalized * Time.fixedDeltaTime * _movementSpeed * _incrementVelocityPerk.CurrentBoost);
    }
}