using UnityEngine;

[CreateAssetMenu(menuName = "Perk/GoldBoostPerk")]
public class GoldBoostPerk : PerkData
{
    [SerializeField] private int _goldBoostPerUpgrade = 5;

    public override void ApplyEffect()
    {
        PerkSkillManager.Instance.ChangeGold(_goldBoostPerUpgrade);
        PerkSkillManager.Instance.Save();
    }

    public override bool BuyPerk()
    {
        bool purchased = base.BuyPerk();

        if (purchased)
        {
            ApplyEffect();
        }

        return purchased;
    }
}