using UnityEngine;

public class FactorySimpleBullet : AbstractFactory<AbstractBullet>
{
    public static FactorySimpleBullet Instance;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        base.Awake();
    }

    public override AbstractBullet Create()
    {
        var x = _pool.Get();
        x.Initialize(_pool);
        return x;
    }

    protected override AbstractBullet CreatePrefab()
    {
        var b = Instantiate(_prefab);
        return b;
    }

    protected override void TurnOff(AbstractBullet obj)
    {
        obj.Refresh();
        obj.gameObject.SetActive(false);
    }

    protected override void TurnOn(AbstractBullet obj)
    {
        obj.gameObject.SetActive(true);
    }
}