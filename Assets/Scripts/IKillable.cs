using System;

public interface IKillable
{
    public static event Action OnDead;
    public void Die();
}