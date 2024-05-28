using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    private float _currentSpeed;
    public float MoveSpeed = 5f;
    public float RunSpeed = 15;
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _currentSpeed = RunSpeed;
            _animator.SetBool("Run", true);
        }
        else
        {
            _currentSpeed = MoveSpeed;
            _animator.SetBool("Run", false);
        }

        _yVelocity += _gravity * Time.deltaTime;
        dir.y = _yVelocity;
        _currentSpeed = MoveSpeed;
        _characterController.Move(dir * _currentSpeed * Time.deltaTime);
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
