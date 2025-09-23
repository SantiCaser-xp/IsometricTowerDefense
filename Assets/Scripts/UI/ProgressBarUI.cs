using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ProgressBarUI : MonoBehaviour
{
    [Header("UI refs")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected Image fillImage;
    [SerializeField] protected Image backgroundImage;
    [SerializeField] protected TMP_Text valueText;

    [Header("Options")]
    [SerializeField] private bool showNumbers = false;
    [SerializeField] private bool roundToInt = true;
    [SerializeField] private string numbersFormat = "{0}/{1}";

    protected virtual void Awake()
    {
        ConfigureSlider();
    }

    protected virtual void OnValidate()
    {
        if (slider != null)
        {
            if (fillImage == null && slider.fillRect != null)
                fillImage = slider.fillRect.GetComponent<Image>();

            if (backgroundImage == null)
            {
                var imgs = slider.GetComponentsInChildren<Image>(true);
                foreach (var img in imgs)
                {
                    if (img == fillImage) continue;
                    backgroundImage = img;
                    break;
                }
            }
        }
        ConfigureSlider();
    }

    private void ConfigureSlider()
    {
        if (slider == null) return;

        slider.interactable = false;
        slider.transition = Selectable.Transition.None;
        slider.direction = Slider.Direction.LeftToRight;

        if (fillImage != null)
        {
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
    }

    protected void SetProgress(float current, float max)
    {
        if (slider == null) return;
        if (max <= 0f) max = 1f;

        slider.minValue = 0f;
        slider.maxValue = max;
        slider.value = Mathf.Clamp(current, 0f, max);

        if (showNumbers && valueText != null)
        {
            if (roundToInt)
                valueText.text = string.Format(numbersFormat, Mathf.CeilToInt(current), Mathf.CeilToInt(max));
            else
                valueText.text = string.Format(numbersFormat, current.ToString("0.##"), max.ToString("0.##"));
        }
    }

    protected abstract void Subscribe();
    protected abstract void Unsubscribe();

    protected virtual void OnEnable() => Subscribe();
    protected virtual void OnDisable() => Unsubscribe();
}
