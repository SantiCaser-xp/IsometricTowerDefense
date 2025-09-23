using UnityEngine;

public class PreviewCollisionDetector : MonoBehaviour
{
    public bool IsColliding { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            IsColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            IsColliding = false;
    }
}
