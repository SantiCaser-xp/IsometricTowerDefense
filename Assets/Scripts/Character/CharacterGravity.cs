using UnityEngine;

public class CharacterGravity : MonoBehaviour
{
    [SerializeField] float _radius;
    [SerializeField] float _force;
    [SerializeField] float _maxSpeed = 7f;
    [SerializeField] LayerMask _layerMask;
    Rigidbody _rb;
    Collider[] _detectedObj;
    int maxColliders = 10;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _detectedObj = new Collider[maxColliders];    
    }

    void FixedUpdate()
    {
        int numbersCollider = Physics.OverlapSphereNonAlloc(transform.position, _radius, _detectedObj, _layerMask);

        for (int i = 0; i < numbersCollider; i++)
        {
            var targetRb = _detectedObj[i].attachedRigidbody;

            Vector3 dir = (_rb.position - targetRb.position);
            float distance = dir.magnitude;

            if (distance < 0.1f) continue;

            dir.Normalize();

            targetRb.AddForce(dir * _force, ForceMode.Acceleration);

            targetRb.velocity = Vector3.ClampMagnitude(
                targetRb.velocity,
                _maxSpeed
            );
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}