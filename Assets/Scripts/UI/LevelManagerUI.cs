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

    public void UpdateData(params object[] values)
    {
        int currentEnemy = (int)values[0];
        int maxEnemy = (int)values[1];  

        enemiesToKillText.text = $"Enemies Killed: {currentEnemy} / {maxEnemy}";
    }
}