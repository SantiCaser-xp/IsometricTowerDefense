using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_TestDamageable
{
    public void TakeDamage(float damageAmount);
    event System.Action<I_TestDamageable> OnDeath;
}
