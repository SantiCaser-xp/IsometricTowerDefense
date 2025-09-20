using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastSphere : MonoBehaviour
{
    [Header("Size setting")]
    [SerializeField] private float scaleA = 1f; // Start
    [SerializeField] private float scaleB = 5f; // Final
    [SerializeField] private float timeT = 2f;  // time

    private float currentTime = 0f;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private bool isExpanding = true;

    void Start()
    {

        initialScale = Vector3.one * scaleA;
        targetScale = Vector3.one * scaleB;

        transform.localScale = initialScale;


        currentTime = 0f;
    }

    void Update()
    {
        if (isExpanding)
        {

            currentTime += Time.deltaTime;


            float progress = currentTime / timeT;


            transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);


            if (progress >= 1f)
            {

                transform.localScale = targetScale;


                Destroy(gameObject);
            }
        }
    }
}
