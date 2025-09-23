using System.Collections;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private Coroutine lerpCoroutine;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);

        // Asegura que el preview tenga el detector
        if (previewObject.GetComponent<PreviewCollisionDetector>() == null)
            previewObject.AddComponent<PreviewCollisionDetector>();

        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);

        // Inicia la animación de los sliders
        if (lerpCoroutine != null)
            StopCoroutine(lerpCoroutine);
        lerpCoroutine = StartCoroutine(LerpMaterialSliders());
    }


    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }
    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if (previewObject != null)
            Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }
        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.SetColor("Color", c);
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }

    public PreviewCollisionDetector GetPreviewCollisionDetector()
    {
        if (previewObject == null) return null;
        return previewObject.GetComponent<PreviewCollisionDetector>();
    }

    private IEnumerator LerpMaterialSliders()
    {
        //Debug.Log("Starting LerpMaterialSliders Coroutine");
        float duration = 0.3f;
        float timer = 0f;
        float DisplacementInitial = 0.800f;
        float DistortionInitial = 49.5f;
        float DisplacementFinal = 0.13f;
        float DistortionFinal = 3.4f;

        previewMaterialInstance.SetFloat("_DisplacementAmount", DisplacementInitial);
        previewMaterialInstance.SetFloat("_DistortionFrequency", DistortionInitial);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float valueA = Mathf.Lerp(DisplacementInitial, DisplacementFinal, t);
            float valueB = Mathf.Lerp(DistortionInitial, DistortionFinal, t);

            previewMaterialInstance.SetFloat("_DisplacementAmount", valueA);
            previewMaterialInstance.SetFloat("_DistortionFrequency", valueB);

            yield return null;
        }

        previewMaterialInstance.SetFloat("_DisplacementAmount", DisplacementFinal);
        previewMaterialInstance.SetFloat("_DistortionFrequency", DistortionFinal);
    }
}
