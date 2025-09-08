using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    [SerializeField] private SimpleTowerBullet bulletPrefab;
    [SerializeField] private int initialPoolSize = 10;

    private ObjectPool<SimpleTowerBullet> pool;

    private void Awake()
    {
        pool = new ObjectPool<SimpleTowerBullet>(bulletPrefab, initialPoolSize, this.transform);
    }

    public SimpleTowerBullet GetBullet()
    {
        return pool.Get();
    }

    public void ReturnBullet(SimpleTowerBullet bullet)
    {
        pool.ReturnToPool(bullet);
    }
}
