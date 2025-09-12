using UnityEngine;

public class CharacterMeshRotator : MonoBehaviour
{
    [SerializeField] private float _speedRotation = 10f;

    public void RotateMesh(Vector3 input)
    {
        Quaternion targetRotation = Quaternion.LookRotation(input);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _speedRotation * Time.fixedDeltaTime);
    }
}