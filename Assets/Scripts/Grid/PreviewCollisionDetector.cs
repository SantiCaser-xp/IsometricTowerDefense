using UnityEngine;

public class PreviewCollisionDetector : MonoBehaviour
{
    public bool IsColliding { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")|| other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            IsColliding = true;
            //Debug.Log("Collision Detected with " + other.gameObject.layer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")|| other.gameObject.layer == LayerMask.NameToLayer("Object"))
            IsColliding = false;
    }
}
