using UnityEngine;

public class BorderLimiter : MonoBehaviour
{
    [SerializeField] LevelBoundary _levelBoundary;
    Rigidbody _rb;

    void Awake()
    {
       _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        /* if (transform.position.magnitude > _levelBoundary.Radius)
         {
             if (_levelBoundary.LimitMode == BorderType.Limit)
             {
                 transform.position = transform.position.normalized * _levelBoundary.Radius;
             }

             if (_levelBoundary.LimitMode == BorderType.Teleport)
             {
                 transform.position = -transform.position.normalized * _levelBoundary.Radius;
             }
         }*/

        Vector3 center = _levelBoundary.transform.position;
        Vector3 pos = _rb.position;

        if ((pos - center).magnitude > _levelBoundary.Radius)
        {
            Vector3 dir = (pos - center).normalized;

            Vector3 newPos;

            if (_levelBoundary.LimitMode == BorderType.Limit)
                newPos = center + dir * _levelBoundary.Radius;
            else
                newPos = center - dir * _levelBoundary.Radius;

            _rb.MovePosition(newPos);
        }
    }

    
}