using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KamikazeAttackState : EnemyState<EnemyFSMStates, MVC_Enemy>
{
    private float _timer;
    private bool _isExploded = false;

    private const float EXPLOSION_DELAY = 0.5f;

    public override void OnEnter()
    {
        avatar.Model.StopMovement();
        _timer = 0;
        _isExploded = false;
    }
    public override void OnExecute()
    {
        if (avatar.Model.CurrentTarget == null)
        {
            enemyFSM.ChangeState(EnemyFSMStates.Idle);
        }

        avatar.Model.RotateTowardTarget();

        _timer += Time.deltaTime;

        if (_timer >= EXPLOSION_DELAY && !_isExploded)
        {
            _isExploded = true;
            Explode();
        }
    }
    private void Explode()
    {
        avatar.Model.PerformAttack();
      avatar.Model.Die();  // to change state to die
        
    }

}
