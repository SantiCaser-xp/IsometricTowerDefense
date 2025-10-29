using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greanade : MonoBehaviour
{
    Vector3 startPos;
    Vector3 targetPos;
    float height;
    float duration;
    float t;
    [SerializeField] float radius = 5f;
    [SerializeField] LayerMask _enemyMask;

    public void Init(Vector3 start, Vector3 target, float height, float duration)
    {
        startPos = start;
        targetPos = target;
        this.height = height;
        this.duration = duration;
        t = 0;
    }

    void Update()
    {
        if (t < 1)
        {
            t += Time.deltaTime / duration;

            // Movimiento parabólico
            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);
            pos.y += height * Mathf.Sin(Mathf.PI * t); // parábola simple
            transform.position = pos;

            // Rotar hacia el movimiento
            Vector3 dir = (pos - transform.position).normalized;
            if (dir != Vector3.zero)
                transform.forward = dir;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Placement"))
        {
            Collider[] _hitedEnemies = Physics.OverlapSphere(transform.position, radius, _enemyMask);
            foreach (var hit in _hitedEnemies)
            {
                Debug.Log("Hit enemy: " + hit.name);
                var enemy = hit.GetComponent<IDamageable<float>>();
                if (enemy != null)
                {
                    enemy.TakeDamage(200f);
                }
            }
            Destroy(gameObject);
        }
    }
}
