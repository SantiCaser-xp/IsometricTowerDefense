using UnityEngine;

public class EnemyFactory : AbstractFactory<MVC_Enemy>
{
    [SerializeField] GoldResourseFactory _goldFactory;
    public override MVC_Enemy Create()
    {
        var x = _pool.Get();
        x.Initialize(_pool, _goldFactory);
        return x;
    }

    protected override MVC_Enemy CreatePrefab()
    {
        var b = Instantiate(_prefab);
        return b;
    }

    protected override void TurnOff(MVC_Enemy obj)
    {
        obj.Refresh();
        obj.gameObject.SetActive(false);
    }

    protected override void TurnOn(MVC_Enemy obj)
    {
        obj.gameObject.SetActive(true);
    }
}