using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    public float MoveSpeed = 5f;
    private CharacterController _characterController;
    private Animator _animator;

    private float _gravity = -9.8f;
    private float _yVelocity = 0f;

    public float JumpPower = 2.5f;
    private bool _isJumping = false;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (!_owner.PhotonView.IsMine)
        {
            return;
        }
        HandleMovement();
        OnAnimatorMove();
    }
    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        Vector3 unNormalizedDir = dir;
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir);

        if (_characterController.isGrounded)
        {
            _isJumping = false;
            _yVelocity = 0;
        }

        if (!_characterController.isGrounded && !_isJumping)
        {
            _isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumping = true;
            _yVelocity = JumpPower;
            dir.y = _yVelocity;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _animator.SetTrigger("Punching");
        }

        _yVelocity += _gravity * Time.deltaTime;
        dir.y = _yVelocity;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _animator.SetFloat("Move", unNormalizedDir.magnitude);
    }

    private void OnAnimatorMove()
    {
        if (_owner.PhotonView.IsMine)
        {
            _characterController.Move(_animator.deltaPosition);
            transform.rotation = _animator.rootRotation;
        }
    }
}
