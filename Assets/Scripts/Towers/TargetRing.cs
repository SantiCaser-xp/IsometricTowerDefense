using UnityEngine;

public class TargetRing : MonoBehaviour
{
    [SerializeField] private GameObject mesh;
    private bool isActive = false;
    private Transform target;

    private void Update()
    {
        if (isActive && target != null)
        {
            transform.position = new Vector3(target.position.x, 0 , target.position.z);
        }
    }

    public void RingActive(Transform target)
    {
        isActive = true;
        this.target = target;
        mesh.SetActive(true);
        transform.position = target.position;
    }

    public void Hide()
    {
        mesh.SetActive(false);
        isActive = false;
        target = null;
    }
}
