using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool _jumpTriggered = false; // 점프 애니메이션이 트리거되었는지 확인

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
        if (isFallGuysScene && FallGuysManager.Instance._currentGameState == GameState.Loading)
        {
            return; // 로딩 중일 때 입력을 무시합니다.
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        Vector3 unNormalizedDir = dir;
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir);

        if (_characterController.isGrounded)
        {
            _isJumping = false;
            _jumpTriggered = false;
            _yVelocity = 0;
        }

        if (!_characterController.isGrounded)
        {
            _isJumping = true;
            _animator.SetBool("RunJump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _animator.SetTrigger("Jump");
            _jumpTriggered = true;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _animator.SetTrigger("Punching");
        }

        if (isFallGuysScene)
        {
            if (_animator.GetFloat("Move") >= 1)
            {
                _IsRunning = true;
                _animator.SetBool("Run", true);
            }
            else
            {
                _IsRunning = false;
                _animator.SetBool("Run", false);
            }
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
                _IsRunning = false;
                _currentSpeed = MoveSpeed;
                _animator.SetBool("Run", false);
            }
        }

        _yVelocity += _gravity * Time.deltaTime;
        dir.y = _yVelocity;
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

    public void PerformJump()
    {
        if (_jumpTriggered)
        {
            _isJumping = true;
            _yVelocity = JumpPower;
        }
    }
}
