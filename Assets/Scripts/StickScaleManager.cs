using System.Collections;
using System.Collections.Generic;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class StickScaleManager : MonoBehaviour
{
    public float myScale = 1f;
    private float targetScale;
    private float lerpSpeed = 5f; // Puedes ajustar la velocidad

    void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += UpdateData;
        targetScale = myScale;
        transform.localScale = new Vector3(myScale, myScale, myScale);
    }

    public void UpdateData(ConfigResponse configResponse)
    {
        targetScale = RemoteConfigService.Instance.appConfig.GetFloat("StickScale");
        myScale = transform.localScale.x;
        StartCoroutine(ChangeScale());
        // No se cambia directamente la escala aquí
    }

    public IEnumerator ChangeScale()
    {
        Debug.Log("Cambiando escala a: " + targetScale);
        while (Mathf.Abs(transform.localScale.x - targetScale) > 0.01f)
        {
            float newScale = Mathf.Lerp(transform.localScale.x, targetScale, Time.deltaTime * lerpSpeed);
            transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
        }
        // Asegura que la escala final sea exacta
        transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }
}
