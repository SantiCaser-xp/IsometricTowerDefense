using UnityEngine;

[CreateAssetMenu(menuName = "Perk/HealthBoostPerk")]
public class HealthBoostPerk : PerkData
{
    [SerializeField] private float _healthBoostPerUpgrade = 10;

    public override void ApplyEffect()
    {
        PerkSkillManager.Instance.ChangeHealth(_healthBoostPerUpgrade);
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