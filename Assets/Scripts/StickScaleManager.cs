using System.Collections;
using System.Collections.Generic;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class StickScaleManager : MonoBehaviour
{
    public float myScale = 1f;
    private float targetScale;
    private float lerpSpeed = 5f;

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
    }

    private void OnDisable()
    {
        RemoteConfigService.Instance.FetchCompleted -= UpdateData;
    }

    public IEnumerator ChangeScale()
    {
        while (Mathf.Abs(transform.localScale.x - targetScale) > 0.01f)
        {
            float newScale = Mathf.Lerp(transform.localScale.x, targetScale, Time.deltaTime * lerpSpeed);
            transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
        }
        transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }
}
