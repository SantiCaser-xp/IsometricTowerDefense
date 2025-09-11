using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private float _speedRate;
    [SerializeField] private Vector3 _offset;

    private void Start()
    {
        transform.position = _character.transform.position + _offset;
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = _character.transform.position + _offset;

        transform.position = Vector3.Lerp(transform.position, targetPos, _speedRate * Time.fixedDeltaTime);
        transform.LookAt(_character.transform);
    }
}