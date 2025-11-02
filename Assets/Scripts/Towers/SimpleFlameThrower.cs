using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlameThrower : AbstractTower
{
    public float viewRadius;
    public float viewAngle;

    [SerializeField] private Transform meshToRotate;

    public List<GameObject> viewableEntities;

    private Coroutine damageCoroutine;

    public LayerMask wallLayer;

    protected override void Update()
    {
        base.Update();
        FieldOfView();
    }

    //Aim at target
    void FieldOfView()
    {
        if (enemiesInRange.Count == 0)
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
            return;
        }
        else
        {
            meshToRotate.forward = Vector3.Lerp(meshToRotate.forward, (enemiesInRange[0] as MonoBehaviour).transform.position - meshToRotate.position, 0.5f * Time.deltaTime);
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamageOverTime());
            }
        }
    }
    public IEnumerator DamageOverTime()
    {
        while (enemiesInRange.Count > 0)
        {
            Debug.Log("<color=red>Dealing Damage</color>");

            foreach (var item in enemiesInRange)
            {
                MonoBehaviour mb = item as MonoBehaviour;
                Vector3 dir = mb.transform.position - transform.position;
                if (dir.magnitude > viewRadius) continue;

                if (Vector3.Angle(meshToRotate.forward, dir) <= viewAngle / 2)
                {
                    item.TakeDamage(damage);
                }
            }
            yield return new WaitForSeconds(1f);
        }
        damageCoroutine = null; // Corrutina termina, limpiamos la referencia
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(meshToRotate.position, viewRadius);

        Vector3 lineA = GetVectorFromAngle(viewAngle / 2 + meshToRotate.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-viewAngle / 2 + meshToRotate.eulerAngles.y);

        Gizmos.DrawLine(meshToRotate.position, meshToRotate.position + lineA * viewRadius);
        Gizmos.DrawLine(meshToRotate.position, meshToRotate.position + lineB * viewRadius);
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
