using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요

public class PlayerMoveAbility : PlayerAbility
{
    private float _currentSpeed;
    public float MoveSpeed = 5f;
    public float RunSpeed = 15;

    public bool _IsRunning;

    private CharacterController _characterController;
    private Animator _animator;

    private float _gravity = -9.8f;
    private float _yVelocity = 0f;

    public float JumpPower = 2.5f;
    private bool _isJumping = false;

    private bool isFallGuysScene = false; // FallGuysScene 여부 확인

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        // 현재 씬이 FallGuysScene인지 확인
        isFallGuysScene = SceneManager.GetActiveScene().name == "FallGuysScene";
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
            _animator.SetBool("RunJump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumping = true;
            _yVelocity = JumpPower;
            dir.y = _yVelocity;
            if (_IsRunning == true)
            {
                _animator.SetBool("RunJump", true);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _animator.SetTrigger("Punching");
        }

        if (isFallGuysScene)
        {
            _IsRunning = true;
            _animator.SetBool("Run", true);
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _IsRunning = true;
                _currentSpeed = RunSpeed;
                _animator.SetBool("Run", true);
            }
            else
            {
                _IsRunning= false;
                _currentSpeed = MoveSpeed;
                _animator.SetBool("Run", false);
            }
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
