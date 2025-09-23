using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTower : Destructible
{
    [SerializeField] protected AbstractFactory<AbstractBullet> _factory;

    protected virtual void Shoot(IDamageable<float> target, Transform targetTransform) { }
}