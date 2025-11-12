using UnityEngine;

[CreateAssetMenu(menuName = "Perks/PerkData")]
public abstract class PerkData : ScriptableObject
{
    public Sprite Icon;
    public int BasePrice = 1;

    public virtual bool BuyPerk()
    {
        if (ExperienceSystem.Instance.CurrentPerksCount >= BasePrice)
        {
            ExperienceSystem.Instance.SubstractPerk(BasePrice);
            EventManager.Trigger(EventType.OnPerkChanged);
            return true;
        }

        return false;
    }

    public abstract void ApplyEffect();
}