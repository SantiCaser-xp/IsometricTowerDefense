using UnityEngine;

public abstract class AbstractTower : Destructible
{
    [SerializeField] protected AbstractFactory<AbstractBullet> _factory;

    public virtual void Shoot(IDamageable<float> target, Transform targetTransform) { }                                    
}