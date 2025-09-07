using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : EnemyState<EnemyFSMStates, BaseEnemy>
{
    public override void OnEnter()
    {
        Debug.Log($"Enter a DieState");
    }

    public override void OnExecute()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            enemyFSM.ChangeState(EnemyFSMStates.Idle);
        }
    }
    public override void OnExit()
    {
        Debug.Log($"Exit from DieState");
    }
}

