using UnityEngine;

[CreateAssetMenu(menuName = "Perk/VelocityBoostPerk")]
public class VelocityBoostPerk : PerkData
{
    [SerializeField] private float _speedBoostPerUpgrade = 1.2f;

    public override void ApplyEffect()
    {
        PerkSkillManager.Instance.ChangeCharacterSpeed(_speedBoostPerUpgrade);
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