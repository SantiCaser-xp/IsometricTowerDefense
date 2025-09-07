
using UnityEngine;

public interface ITargetable 
{
   Vector3 Position { get; }
    float CurrentHealth {  get; }
    float MaxHealth {  get; }
    bool IsAlive { get; }
    TargetType TargetType { get; }
    GameObject GameObject { get; }
    void TakeDamage(float damage);
        
}
