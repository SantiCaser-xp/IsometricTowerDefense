using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoVAgent : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;

    public List<GameObject> viewableEntities;

    public LayerMask wallLayer;

    void Update()
    {
        FieldOfView();
    }

    void FieldOfView()
    {
        foreach (var item in viewableEntities)
        {


            Vector3 dir = item.transform.position - transform.position;
            if (dir.magnitude > viewRadius) continue;

            if (Vector3.Angle(transform.forward, dir) <= viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, dir, out RaycastHit hit, dir.magnitude, wallLayer))
                {
                    Debug.DrawLine(transform.position, item.transform.position, Color.red);
                    item.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.white);

                }
            }
            else
            {
                item.gameObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 lineA = GetVectorFromAngle(viewAngle / 2 + transform.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-viewAngle / 2 + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + lineA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineB * viewRadius);


    }

    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }


}
