using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCollDetector : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Material material;
    public bool isColliding;

    private void Update()
    {
        if (!isColliding)
        {
            Color currentColor = material.color;
            material.color = new Color(1f, 1f, 1f, currentColor.a);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding with: " + other);
        if (((1 << other.gameObject.layer) & layerMask.value) != 0)
        {
            Debug.Log("Colliding with object");
            isColliding = true;
            Color currentColor = material.color;
            material.color = new Color(1f, 0f, 0f, currentColor.a); // Rojo, alpha original
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask.value) != 0)
        {
            isColliding= false;
        }
    }
}
