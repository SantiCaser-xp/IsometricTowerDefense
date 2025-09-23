using UnityEngine;

public interface ITargetable 
{
    public TargetType TargetType { get; }
    public Vector3 GetPos();
}