using UnityEngine;

[CreateAssetMenu(menuName = "Perk/GoldBoostPerk")]
public class GoldBoostPerk : PerkData
{
    [SerializeField] private int _goldBoostPerUpgrade = 5;

    public override void ApplyEffect(PerkInstance instance, GameObject player)
    {
        var deposit = player.GetComponent<CharacterDeposit>();
        if (deposit != null)
        {
            deposit.ChangeStartedDeposit(_goldBoostPerUpgrade);
        }
    }

    public override void RemoveEffect(PerkInstance instance, GameObject player)
    {
        var deposit = player.GetComponent<CharacterDeposit>();
        if (deposit != null)
        {
            deposit.ChangeStartedDeposit(-_goldBoostPerUpgrade);
        }
    }
}