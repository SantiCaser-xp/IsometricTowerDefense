using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MoveState : EnemyState<EnemyFSMStates, BaseEnemy>
{
    private int _actualWaypoint = 0;
    private float _attackCoolDown;
    public override void OnEnter()
    {
        Debug.Log($"Debug Enter a MoveState");
    }

    public override void OnExecute()
    {
        
        _attackCoolDown -= Time.deltaTime;
        if (avatar.ReduceEnergy(Time.deltaTime * 5))
        {
            enemyFSM.ChangeState(EnemyFSMStates.Idle);
        }

        //From Patrol to Chase
        var closeTarget = Physics.OverlapSphere(avatar.transform.position, avatar.detectionRadius, avatar.targetMask);
        if (closeTarget.Length > 0)
        {
            avatar.SetTarget(closeTarget[0].gameObject);
            //enemyFSM.ChangeState(EnemyFSMStates.Move);
            return;
        }

        var chaseVector = avatar._target.transform.position - avatar.transform.position;
        if (chaseVector.magnitude > 0.3f)
        {
            avatar.transform.position += chaseVector.normalized * Time.deltaTime * 5;
        }
        else if (_attackCoolDown <= 0) 
        {
            enemyFSM.ChangeState(EnemyFSMStates.Attack);
            _attackCoolDown = 2f;
        }

        if (avatar.MoveTo(_actualWaypoint))
        {

            _actualWaypoint++;
            if (_actualWaypoint >= avatar.maxWaypoints)
            {
                _actualWaypoint = 0;
            }
        }

    }
    public override void OnExit()
    {
        avatar.SetTarget(null);
    }
}
