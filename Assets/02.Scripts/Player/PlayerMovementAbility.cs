using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerMovementAbility : PlayerAbility
{
    public float MoveSpeed = 5f;
    public float RunSpeed = 15f;
    public float gravity = -9.81f; // 중력 값
    public bool IsRunning;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private TPSCamera _tpsCamera;
    public Transform CameraRoot;

    public float JumpPower = 2.5f;
    private bool _isGrounded = true;
    private bool _isJumping = false; // 점프 상태 관리
    private bool _isFallGuysScene = false; // 폴가이즈 씬인지 확인

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        if (_owner.PhotonView.IsMine)
        {
            GameObject.FindWithTag("MainCamera").GetComponent<TPSCamera>().target = CameraRoot;
        }

        _isFallGuysScene = SceneManager.GetActiveScene().name == "FallGuysScene";
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 movementInput = new Vector3(h, 0, v);
        float inputMagnitude = movementInput.magnitude;

        _animator.SetFloat("Move", Mathf.Clamp01(inputMagnitude));

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !_isJumping)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _animator.SetTrigger("Punching");
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0, v);

        float movementMagnitude = direction.magnitude;
        _rigidbody.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
        if (movementMagnitude > 0.1f)
        {
            _animator.SetBool("Walk", true);
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            direction = (forward.normalized * v + Camera.main.transform.right * h).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f));

            float speed = IsRunning ? RunSpeed : MoveSpeed;
            _rigidbody.MovePosition(_rigidbody.position + direction * speed * Time.deltaTime);
        }
        else
        {
            _animator.SetBool("Walk", false);
        }

        UpdateRunningState();
    }

    private void UpdateRunningState()
    {
        if (_isFallGuysScene || Input.GetKey(KeyCode.LeftShift))
        {
            IsRunning = true;
            _animator.SetBool("Run", true);
        }
        else
        {
            _animator.SetBool("Run", false);
            IsRunning = false;
        }
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z); // 기존의 y 속도를 0으로 설정
            _rigidbody.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            _animator.SetTrigger(IsRunning ? "RunJump" : "Jump");
            _isGrounded = false;
            _isJumping = true;
        }
    }

    private void ApplyGravity()
    {
        if (!_isGrounded)
        {
            // 중력 적용
            _rigidbody.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("");
        // 충돌한 표면이 수평에 가까운지 확인
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                _isGrounded = true;
                _isJumping = false; // 점프 상태 초기화
                break;
            }
        }
    }
}
