using UnityEngine;

public class ButtonChangeLanguage : MonoBehaviour
{
    public Language language;

    public void BTNChangeLanguage()
    {
        LocalizationManager.instance.ChangeLanguage(language);
    }
}
