using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTower : MonoBehaviour
{
    [SerializeField] protected AbstractFactory<AbstractBullet> _factory;
    public abstract void Shoot(I_TestDamageable target, Transform targetTransform);
}
