using UnityEngine;

public class GoldSpawner : MonoBehaviour
{
    [SerializeField] private GoldResourseFactory _goldFactory;

    public void GetSpawn()
    {
        _goldFactory.Create();
    }
}
