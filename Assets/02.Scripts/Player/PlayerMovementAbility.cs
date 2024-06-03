using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerMovementAbility : PlayerAbility
{
    private float _currentSpeed;
    public float MoveSpeed = 5f;
    public float RunSpeed = 15f;

    public bool _isRunning;

    private CharacterController _characterController;
    private Animator _animator;

    private float _gravity = -9.8f;
    private float _yVelocity = 0f;

    public float JumpPower = 2.5f;
    private bool _isJumping = false;

    private bool _isFallGuysScene = false; // 폴가이즈 씬인지 확인

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _isFallGuysScene = SceneManager.GetActiveScene().name == "FallGuysScene";
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine)
        {
            return;
        }

        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0, v);
        float movementMagnitude = direction.magnitude;

        _animator.SetFloat("Move", Mathf.Clamp01(movementMagnitude));

        if (movementMagnitude > 0.1f)
        {
            _animator.SetBool("Walk", true);

            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0f;
            direction = forward.normalized * v + Camera.main.transform.right * h;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            _characterController.Move(direction.normalized * _currentSpeed * Time.deltaTime);
        }
        else
        {
            _animator.SetBool("Walk", false);
        }

        if (_isFallGuysScene)
        {
            _currentSpeed = RunSpeed;
            if (_animator.GetBool("Walk"))
            {
                _isRunning = true;
                _animator.SetBool("Run", true);
            }
            else
            {
                _isRunning = false;
                _animator.SetBool("Run", false);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _isRunning = true;
                _currentSpeed = RunSpeed;
                _animator.SetBool("Run", true);
            }
            else
            {
                _isRunning = false;
                _currentSpeed = MoveSpeed;
                _animator.SetBool("Run", false);
            }
        }

        if (_characterController.isGrounded)
        {
            _isJumping = false;
            _yVelocity = -0.5f; // 살짝 아래로 향하게 함
        }
        else
        {
            _isJumping = true;
            _animator.SetBool("RunJump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            Jump();
            if (_isRunning)
            {
                _animator.SetBool("RunJump", true);
            }
            else
            {
                _animator.SetTrigger("Jump");
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _animator.SetTrigger("Punching");
        }
    }

    private void ApplyGravity()
    {
        _yVelocity += _gravity * Time.deltaTime;
        Vector3 velocity = new Vector3(0, _yVelocity, 0);
        _characterController.Move(velocity * Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        if (_owner.PhotonView.IsMine)
        {
            _characterController.Move(_animator.deltaPosition);
            transform.rotation = _animator.rootRotation;
        }
    }

    public void Jump()
    {
        _isJumping = true;
        _yVelocity = JumpPower;
    }
}
