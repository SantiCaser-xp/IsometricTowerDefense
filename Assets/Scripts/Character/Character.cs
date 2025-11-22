using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Character : MonoBehaviour
{
    [SerializeField] private CharacterMeshRotator _meshRotator;
    [SerializeField] private ControlBase _joystick;
    private CharacterInputController _controller;
    private CharacterMovement _movement;
    private CharacterAnimationController _animationController;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _animationController = new CharacterAnimationController(_animator);
        _controller = new CharacterInputController(_joystick);
        _movement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        _controller.InputArtificialUpdate();
        
        _animationController.ChangeVelocity(_controller.InputDirection.magnitude);
    }

    private void FixedUpdate()
    {
        if(_controller.InputDirection.sqrMagnitude > 0.001f)
        {
            _movement.Movement(_controller.InputDirection);
            _meshRotator.RotateMesh(_controller.InputDirection);
        }
    }
}