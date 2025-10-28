using UnityEngine;

public class CharacterAnimationController
{
    [SerializeField] string _currentHealthName = "currentHealth";
    [SerializeField] string _currentVelocityName = "currentVelocity";
    [SerializeField] string _isFunnyBoolName = "isFunny";
    [SerializeField] string _isHallooweenBoolName = "isHalloween";
    [SerializeField] string _isBuildedTriggerName = "isBuilded";
    private Animator _animator;

    public CharacterAnimationController(Animator animator)
    {
        _animator = animator;
    }

    public void ChangeVelocity(float dir)
    {
        _animator.SetFloat(_currentVelocityName, dir);
    }

    public void ChangeHealth(float health)
    {
        _animator.SetFloat(_currentHealthName, health);
    }

    public void ChangeFunnyMode(bool value)
    {
        _animator.SetBool(_isFunnyBoolName, value);
    }

    public void ChangeHalloweenMode(bool value)
    {
        _animator.SetBool(_isHallooweenBoolName, value);
    }

    public void ActivateTrigger()
    {
        _animator.SetTrigger(_isBuildedTriggerName);
    }
}