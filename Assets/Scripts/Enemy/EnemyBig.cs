
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyBig : BaseEnemy

{
    [Header("Shoot")]
    [SerializeField] private Transform firePoint;
    public override void Start()
    {
        base.Start();
        _factory = FactorySimpleBullet.Instance;
       
    }
    public override void PerformAttack()
    {
        if (currentTarget != null)//&& currentTarget.IsAlive)
        {
            
            //projectile will shoot 
            var bullet = _factory.Create();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet._damage = damage;
            bullet.SetTarget(currentTarget, currentTarget.GetPos());
            bullet._isShooted = true;
        }
    }
}
