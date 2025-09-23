using System;
using UnityEngine;

public class EnemyFactory : AbstractFactory<BaseEnemy>
{
    [SerializeField] GoldResourseFactory _goldFactory;
    public override BaseEnemy Create()
    {
        var x = _pool.Get();
        x.Initialize(_pool, _goldFactory);

        Debug.Log("Creating enemy from pool: " + x.ToString());

        return x;
    }

    protected override BaseEnemy CreatePrefab()
    {
        var b = Instantiate(_prefab);
        return b;
    }

    protected override void TurnOff(BaseEnemy obj)
    {
        obj.Refresh();
        obj.gameObject.SetActive(false);
    }

    protected override void TurnOn(BaseEnemy obj)
    {
        obj.gameObject.SetActive(true);
    }


}
