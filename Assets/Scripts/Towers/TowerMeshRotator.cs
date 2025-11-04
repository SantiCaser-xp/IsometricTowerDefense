using UnityEngine;


public class TowerMeshRotator : MonoBehaviour
{
    [SerializeField] private float _speedRotation = 5f;
    [SerializeField] private float _angleThreshold = 5f;
    private Quaternion _randomRot;

    private void Start()
    {
        InvokeRepeating(nameof(SetRandomRotation), 0, 5);
    }

    public void RotateTowerToEnemy(Transform target)
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        dir.y = 0f;

        if(dir.sqrMagnitude > 0.3f * 0.3f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, _speedRotation * Time.deltaTime);
        }

        //if (dir.sqrMagnitude > 0.3f * 0.3f)
        //{
        //    Quaternion targetRot = Quaternion.LookRotation(dir);
        //    float maxDegreesDelta = _speedRotation * Time.deltaTime * 100f; // Ajusta el factor según lo necesario
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, maxDegreesDelta);
        //}
    }

    private void SetRandomRotation()
    {
        float randomY = Random.Range(0f, 360f);
        _randomRot = Quaternion.Euler(0, randomY, 0);
    }

    public void RotateTowerIdle()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _randomRot, _speedRotation * Time.deltaTime);
    }

    public bool IsFacingTarget(Transform target)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.3f * 0.3f) return false;
        Debug.Log("Direction to target: " + target + " " + dir.sqrMagnitude);

        Quaternion targetRot = Quaternion.LookRotation(dir);
        float angle = Quaternion.Angle(transform.rotation, targetRot);
        return angle < _angleThreshold;
    }
}