using UnityEngine;

public class SimpleGreanade : AbstractGreanade
{
    //Executed by animation event

    public void DestroyGO()
    {
        _myPool.Release(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}