using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreanadeFactory : AbstractFactory<AbstractGreanade>
{
    public static GreanadeFactory Instance;

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

    public override AbstractGreanade Create()
    {
        var x = _pool.Get();
        x.Initialize(_pool);
        return x;
    }

    protected override AbstractGreanade CreatePrefab()
    {
        var b = Instantiate(_prefab);
        return b;
    }

    protected override void TurnOff(AbstractGreanade obj)
    {
        obj.Refresh();
        obj.gameObject.SetActive(false);
    }

    protected override void TurnOn(AbstractGreanade obj)
    {
        obj.gameObject.SetActive(true);
    }
}
