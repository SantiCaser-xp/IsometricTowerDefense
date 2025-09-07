using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState<EnemyFSMStates, BaseEnemy>
{
    // Energy logic - skip for now
    //private float _actualCounter;
    public override void OnEnter()
    {
        avatar.agent.isStopped = true;
       
        // Energy logic - skip for now
        //_actualCounter = 5;
    }

    public override void OnExecute()
    {
        // Set target logic

        if(Time.time - avatar.lastSearchTime >= avatar.searchInterval)
        {
            avatar.lastSearchTime = Time.time;
            avatar.SearchForTarget();
         
        }
        
      
        if (avatar.currentTarget!=null && avatar.currentTarget.IsAlive)
        {
                      enemyFSM.ChangeState(EnemyFSMStates.Move);
        }

        //var closeTarget = Physics.OverlapSphere(avatar.transform.position, avatar.detectionRadius, avatar.targetMask);
        //if (closeTarget.Length > 0)
        //{
        //    avatar.SetTarget(closeTarget[0].gameObject);
        //    enemyFSM.ChangeState(EnemyFSMStates.Move);
        //    return;
        //}



        // Energy logic - skip for now
        //_actualCounter -= Time.deltaTime;
        //if (_actualCounter <= 0)
        //{
        //    avatar.RecoverEnergy();
        //    enemyFSM.ChangeState(EnemyFSMStates.Move);

        //}

    }
    public override void OnExit()
    {
        Debug.Log($"Exit from IdleState");
    }
}
