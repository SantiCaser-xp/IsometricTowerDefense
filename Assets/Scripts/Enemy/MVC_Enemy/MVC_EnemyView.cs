using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVC_EnemyView
{
    //depencies
    private MVC_EnemyModel _model;
    private Animator _animator;
    private ParticleSystem _particleDmg;
    private AudioSource _soundDmg;

    //constructor
    public MVC_EnemyView(MVC_EnemyModel model, Animator animator, ParticleSystem particleDmg, AudioSource soundDmg)
    {
        _model = model;
        _animator = animator;
        _particleDmg = particleDmg;
        _soundDmg = soundDmg;

        // subscribe on events
        _model.OnTakeDamage += HandleTakeDamage;
        _model.OnAttack += HandleAttack;
        _model.OnDie += HandleDie;
        _model.OnSetMoving += HandleSetMoving;
    }
    private void HandleTakeDamage()
    {
        _animator.SetTrigger("OnHit");  // TODO string -> variable

        _particleDmg?.Play();
        _soundDmg?.Play();
    }
    private void HandleAttack()
    {
        _animator.SetTrigger("OnAttack");
    }
    private void HandleDie()
    {
        _animator.SetTrigger("OnDeath");
    }
    private void HandleSetMoving(bool isMoving)
    {
        _animator.SetBool("isMoving", isMoving);
    }

    //On Destroy
    public void CleanUp()
    {
        // unsubscribe on events
        _model.OnTakeDamage -= HandleTakeDamage;
        _model.OnAttack -= HandleAttack;
        _model.OnDie -= HandleDie;
        _model.OnSetMoving -= HandleSetMoving;
    }
}
