using TMPro;
using UnityEngine;

public class EnemyCounterUI : MonoBehaviour, IObserver
{
    [SerializeField] TextMeshProUGUI _enemyCounterText;

    private void Awake()
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

        _enemyCounterText.SetText($"{currentEnemy}/{maxEnemy}");
    }
}
