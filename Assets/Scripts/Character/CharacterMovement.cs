using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    //[SerializeField] private float _movementSpeed = 3.5f;
    //[SerializeField] private float _movementSpeedMultiplier = 1f;
    private Rigidbody _rb;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Movement(Vector3 dir)
    {
        _moveDirection = (transform.right * dir.x + transform.forward * dir.z);
        _rb.MovePosition(_rb.position + _moveDirection.ToIso() * Time.fixedDeltaTime * PerkSkillManager.Instance.StarCharacterSpeed);//* _movementSpeedMultiplier);
    }

    /*public void ChangeSpeedMultiplier(float speedMultiplier)
    {
        _movementSpeedMultiplier += speedMultiplier;
    }*/

    private void OnCollisionEnter(Collision collision)
    {
    }
}