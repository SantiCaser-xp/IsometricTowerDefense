using UnityEngine;

public class GoldResource : MonoBehaviour
{
    [SerializeField] private int _gold = 1;
    [SerializeField] private CharacterDeposit _deposit;
    private ObjectPool<GoldResource> _pool;

    public void Initialize(ObjectPool<GoldResource> pool)
    {
        _pool = pool;
    }
    public virtual void Refresh() { }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Character>() != null)
        {
            _deposit.AddDeposit(_gold);
            _pool.Release(this);
        }
    }
}