using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState<EnemyFSMStates, BaseEnemy>
{
    private float _actualCounter;
    public override void OnEnter()
    {
        Debug.Log($"Enter a IdleState");
        _actualCounter = 5;
    }

    public override void OnExecute()
    {
        _actualCounter -= Time.deltaTime;
        if (_actualCounter <= 0)
        {
            avatar.RecoverEnergy();
            enemyFSM.ChangeState(EnemyFSMStates.Move);

        }

    }
    public override void OnExit()
    {
        Debug.Log($"Exit from IdleState");
    }
}
