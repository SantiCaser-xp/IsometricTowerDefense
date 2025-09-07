using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState<EnemyFSMStates, BaseEnemy>
{
    public override void OnEnter()
    {
        Debug.Log($"Debug Enter a AttackState");
    }

    public override void OnExecute()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            enemyFSM.ChangeState(EnemyFSMStates.Die);
        }
    }
    public override void OnExit()
    {
        Debug.Log($"Exit from AttackState");
    }
}
