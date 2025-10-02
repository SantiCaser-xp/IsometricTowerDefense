using UnityEngine;

public class GoldResourseFactory : AbstractFactory<GoldResource>
{
    [SerializeField] private CharacterDeposit _deposit;
    public override GoldResource Create()
    {
        var x = _pool.Get();
        x.Initialize(_pool, _deposit);
        //     Debug.Log("Creating bullet from pool: " + x.ToString());

        return x;
    }

    protected override GoldResource CreatePrefab()
    {
        var b = Instantiate(_prefab);
        return b;
    }

    protected override void TurnOff(GoldResource obj)
    {
        obj.Refresh();
        obj.gameObject.SetActive(false);
    }

    protected override void TurnOn(GoldResource obj)
    {
        obj.gameObject.SetActive(true);
    }
}
