using UnityEngine;


public class TowerMeshRotator : MonoBehaviour
{
    [SerializeField] private float _speedRotation = 5f;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private TestEnemy _testEnemy;
    private bool _isIdle = true;
    private float _distance;
    private Quaternion _randomRot;

    private void Start()
    {
        InvokeRepeating(nameof(SetRandomRotation), 0, 5f);
    }

    void Update()
    {
        _distance = (_testEnemy.transform.position - transform.position).sqrMagnitude;

        if(_distance < _radius * _radius)
        {
            _isIdle = false;
        }
        else
        {
            _isIdle = true;
        }

        if (!_isIdle)
        {
            RotateTowerToEnemy();
        }
        else
        {
            RotateTowerIdle();
        }
    }

    private void RotateTowerToEnemy()
    {
        Vector3 dir = (_testEnemy.transform.position - transform.position).normalized;
        dir.y = 0f;

        if(dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, _speedRotation * Time.deltaTime);
        }
    }

    private void SetRandomRotation()
    {
        float randomY = Random.Range(0f, 360f);
        _randomRot = Quaternion.Euler(0, randomY, 0);
    }

    private void RotateTowerIdle()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _randomRot, _speedRotation * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}