using UnityEngine;

public class CharacterGravity : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _force;
    [SerializeField] private LayerMask _layerMask;
    private Rigidbody _rb;
    private Collider[] _detectedObj;
    private int maxColliders = 10;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _detectedObj = new Collider[maxColliders];    
    }

    void FixedUpdate()
    {
        int numbersCollider = Physics.OverlapSphereNonAlloc(transform.position, _radius, _detectedObj, _layerMask);

        for (int i = 0; i < numbersCollider; i++)
        {
            if (_detectedObj[i].attachedRigidbody != null)
            {
                float distance = (_rb.position - _detectedObj[i].attachedRigidbody.position).magnitude;
                Vector3 dir = (_rb.position - _detectedObj[i].attachedRigidbody.position).normalized;
                _detectedObj[i].attachedRigidbody.AddForce(dir * _force / (distance * distance), ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}