using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int _currentPerksCount;
    [SerializeField] private float _currentExperienceThresold;
    [SerializeField] private float _currentExperience;
    public int CurrentPerksCount => _currentPerksCount;
    public float CurrentExperience => _currentExperience;
    public float CurrentExperienceThresold => _currentExperienceThresold;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeExperience(float experience, float experiensThresold)
    {
        _currentExperience = experience;
        _currentExperienceThresold = experiensThresold;
    }

    public void ChangePerks(int perks)
    {
        _currentPerksCount += perks;
    }
}