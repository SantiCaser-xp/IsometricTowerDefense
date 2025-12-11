using UnityEngine;

public class DirtScreen : MonoBehaviour
{
    [SerializeField] Material _dirtScreenMaterial;
    [SerializeField] float _intensity;

    void Start()
    {
        _intensity = 0f;
        _dirtScreenMaterial.SetFloat("_Intensity", _intensity);
    }

    void Update()
    {
        _dirtScreenMaterial.SetFloat("_Intensity", _intensity);
    }
}