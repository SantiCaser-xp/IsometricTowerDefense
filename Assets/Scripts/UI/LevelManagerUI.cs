using TMPro;
using UnityEngine;

public class LevelManagerUI : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI enemiesToKillText;

    void Awake()
    {
        IObservable obs = GetComponentInParent<IObservable>();

        if (obs == null)
        {
            gameObject.SetActive(false);
            return;
        }

        obs.Subscribe(this);
    }

    public void UpdateData(float currentValue, float maxValue)
    {
        enemiesToKillText.text = $"Enemies Killed: {currentValue} / {maxValue}";
    }

    public void UpdateData(int value) { }

    public void UpdateGameStatus(GameStatus status) { }
}