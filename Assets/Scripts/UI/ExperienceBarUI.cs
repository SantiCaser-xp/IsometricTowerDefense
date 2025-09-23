using UnityEngine;

public class ExperienceBarUI : ProgressBarUI
{
    [SerializeField] private ExperienceSystem xp;

    protected override void Subscribe()
    {
        if (xp != null)
            xp.OnXpChanged += OnXp;
    }

    protected override void Unsubscribe()
    {
        if (xp != null)
            xp.OnXpChanged -= OnXp;
    }

    private void Start()
    {
        if (xp != null)
            SetProgress(xp.CurrentXP, xp.XpToNext);
    }

    private void OnXp(int current, int toNext) => SetProgress(current, toNext);
}
