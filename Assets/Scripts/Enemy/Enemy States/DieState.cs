using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DieState : EnemyState<EnemyFSMStates, BaseEnemy>
{
    public override void OnEnter()
    {
        avatar.agent.isStopped = true;
        
    }

    public override void OnExecute()
    {
       

    }

   

    

    public override void OnExit()
    {
       
    }
}

