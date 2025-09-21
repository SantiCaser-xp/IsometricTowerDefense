using UnityEngine;

public class HealthBarUI : ProgressBarUI
{
    [SerializeField] private Character character;

    protected override void Subscribe()
    {
        Character.OnHealthChanged += OnHealth;
    }

    protected override void Unsubscribe()
    {
        Character.OnHealthChanged -= OnHealth;
    }

    private void Start()
    {
        if (character != null)
            SetProgress(character.CurrentHealth, character.MaxHealth);
    }

    private void OnHealth(float current, float max)
    {
        SetProgress(current, max);
    }
}
