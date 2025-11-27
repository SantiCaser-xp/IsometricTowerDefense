using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perks/PerkData")]
public abstract class PerkData : ScriptableObject
{
    public Sprite Icon;
    public int BasePrice = 1;
    public bool IsBought = false;

    public virtual bool BuyPerk()
    {
        if (ExperienceSystem.Instance.CurrentPerksCount >= BasePrice)
        {
            ExperienceSystem.Instance.SubstractPerk(BasePrice);
            EventManager.Trigger(EventType.OnPerkChanged);
            IsBought = true;

            return true;
        }

        return false;
    }

    public abstract void ApplyEffect();
}