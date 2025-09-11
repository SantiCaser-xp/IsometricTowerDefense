using UnityEngine;

[CreateAssetMenu(menuName = "Perk/VelocityBoostPerk")]
public class VelocityBoostPerk : PerkData
{
    [SerializeField] private float _speedBoostPerUpgrade = 0.2f;

    public override void ApplyEffect(PerkInstance instance, GameObject player)
    {
        var movement = player.GetComponent<CharacterMovement>();
        if (movement != null)
        {
            movement.ChangeSpeedMultiplier(_speedBoostPerUpgrade);
        }
    }

    public override void RemoveEffect(PerkInstance instance, GameObject player)
    {
        var movement = player.GetComponent<CharacterMovement>();
        if (movement != null)
        {
            movement.ChangeSpeedMultiplier(_speedBoostPerUpgrade);
        }
    }
}