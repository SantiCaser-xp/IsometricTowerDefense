using UnityEngine;

public class PostEffects : SingltonBase<PostEffects>
{
    [SerializeField] Material _dirtMaterial;
    [SerializeField] Material _rainMaterial;
    [SerializeField] Material _bloodMaterial;
    [SerializeField] Material _desaturateMaterial;
    [SerializeField] Material _heatMaterial;

    void Start()
    {
        ResetEffects();
    }

    

    void OnApplicationQuit()
    {
        ResetEffects();
    }

    void ResetEffects()
    {
        _dirtMaterial.SetFloat("_Intensity", 0f);
        _bloodMaterial.SetFloat("_Intensity", 0f);
        _rainMaterial.SetFloat("_Intensity",0f);
        _heatMaterial.SetFloat("_Intensity", 0f);
        _desaturateMaterial.SetFloat("_Intensity", 1f);
    }
}