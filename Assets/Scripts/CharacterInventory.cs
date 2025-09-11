using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [SerializeField] private int _maxGold = 9999;
    private int _currentGold = 0;

    public void ChangeDeposit(int amount)
    {
        _currentGold += amount;
    }
}