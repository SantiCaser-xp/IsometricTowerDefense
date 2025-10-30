using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultAnim : MonoBehaviour
{
    [SerializeField] private HeavyTurret _heavyTurret;
    public void CallShoot()
    {
        _heavyTurret.CallShoot();
    }
}
