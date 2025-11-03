using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SimpleFlameThrower : AbstractTower
{
    public float viewRadius;
    public float viewAngle;

    [SerializeField] private Transform meshToRotate;

    [SerializeField] private Animator _anim;

    public List<GameObject> viewableEntities;

    private Coroutine damageCoroutine;

    public LayerMask wallLayer;

    [SerializeField] VisualEffect flameVFX;

    [Header("Rotation Settings")]
    [SerializeField] protected TowerMeshRotator _meshTopRotatior;

    private void Start()
    {
        flameVFX.Stop();
    }

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
                flameVFX.Stop();
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
                _anim.SetBool("IsShooting", false);
            }
            return;
        }
        else
        {
            meshToRotate.forward = Vector3.Lerp(meshToRotate.forward, (enemiesInRange[0] as MonoBehaviour).transform.position - meshToRotate.position, 0.5f * Time.deltaTime);
            if (damageCoroutine == null && _meshTopRotatior.IsFacingTarget((enemiesInRange[0] as MonoBehaviour).transform))
            {
                _anim.SetBool("IsShooting", true);
                damageCoroutine = StartCoroutine(DamageOverTime());
                flameVFX.Play();
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
