using UnityEngine;
using System;

public class MVC_EnemyView
{
    //depencies
    private MVC_EnemyModel _model;
    private Animator _animator;
    private ParticleSystem _particleDmg;
    private ParticleSystem _particleAttack;
    private AudioSource _soundDmg;

    //constructor
    public MVC_EnemyView(MVC_EnemyModel model, Animator animator, ParticleSystem particleDmg, ParticleSystem particleAttack, AudioSource soundDmg)
    {
        _model = model;
        _animator = animator;
        _particleDmg = particleDmg;
        _particleAttack = particleAttack;
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


        //_particleDmg?.Play();
        //_particleDie?.Play();
        // _soundDmg?.Play();
    }

    private void HandleAttack()
    {
        Debug.Log("HandleAttack");
        _animator.SetTrigger("OnAttack");

        if (_particleAttack != null)
        {
            Debug.Log("Attack SFX");
            _particleAttack?.Play();
        }
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
