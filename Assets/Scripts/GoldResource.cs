using UnityEngine;

public class GoldResource : MonoBehaviour
{
    [SerializeField] private int _gold = 1;
    private ObjectPool<GoldResource> _pool;
    private CharacterDeposit _characterDeposit;

    public void Initialize(ObjectPool<GoldResource> pool, CharacterDeposit deposit)
    {
        _pool = pool;
        _characterDeposit = deposit;
    }

    public virtual void Refresh() { }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Character>() != null)
        {
            _characterDeposit.AddDeposit(_gold);
            _pool.Release(this);
        }
    }
}