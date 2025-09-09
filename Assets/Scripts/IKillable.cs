using System;

public interface IKillable
{
    public event Action OnDead;
    public void Die();
}