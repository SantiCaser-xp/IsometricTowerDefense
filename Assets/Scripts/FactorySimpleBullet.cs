using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorySimpleBullet : AbstractFactory<AbstractBullet>
{
    public override AbstractBullet Create()
    {
        var b = Instantiate(_prefab);
        return b;
    }
}
