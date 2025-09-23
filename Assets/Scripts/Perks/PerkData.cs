using UnityEngine;

[CreateAssetMenu(menuName = "Perks/PerkData")]
public abstract class PerkData : ScriptableObject
{
    public string Name;
    [TextArea] public string Description;
    public Sprite Icon;

    public int MaxPrice = 3;
    public int MaxUpgradeLevel = 3;
    public int BaseLevel = 1;
    public int BasePrice = 1;

    public abstract void ApplyEffect(PerkInstance instance, GameObject player);
    public abstract void RemoveEffect(PerkInstance instance, GameObject player);
}