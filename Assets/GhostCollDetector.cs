using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCollDetector : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Material material;
    public bool isColliding;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding with: " + other);
        if (((1 << other.gameObject.layer) & layerMask.value) != 0)
        {
            Debug.Log("Colliding with object");
            isColliding = true;
            material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask.value) != 0)
        {
            isColliding= false;
            material.color = Color.white;
        }
    }
}
