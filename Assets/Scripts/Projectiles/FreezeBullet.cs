using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : BulletDecorator
{
    [SerializeField] private ParticleSystem _freezeEffect;
    public FreezeBullet(AbstractBullet bullet)
    {
        _originalBullet = bullet;
    }

    public override void HitTarget(Collider other)
    {
        base.HitTarget(other);
        _freezeEffect.Play();

        //_target.Freeze();  Logica para congelar al objetivo
    }
}
