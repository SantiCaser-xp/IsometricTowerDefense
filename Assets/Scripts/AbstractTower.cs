using UnityEngine;

public abstract class AbstractTower : Destructible, ITargetable, IDamageable<float>
{
    [SerializeField] protected AbstractFactory<AbstractBullet> _factory;

    public TargetType TargetType => TargetType.Tower;


    public abstract void Shoot(IDamageable<float> target, Transform targetTransform);                                       

    public Vector3 GetPos()
    {
        return transform.position;
    }


    public override void Die()
    {
        Destroy(gameObject);
    }
}
