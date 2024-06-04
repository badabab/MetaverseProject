using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveAbility : PlayerAbility
{
    public float Movespeed = 5f;
    public float RunSpeed = 15f;

    public float NormalJumpPower= 3;
    public float RunningJumpPower= 5;
    public float JumpPower;

    public bool isGrounded;		// 땅에 서있는지 체크하기 위한 bool값
    public LayerMask ground;	// 레이어마스크 설정
    public float groundDistance = 0.2f;		// Ray를 쏴서 검사하는 거리

    private bool _isRunning;

    public Transform LayerPoint;
    private Animator _animator;
    Rigidbody rb;
    public Transform CameraRoot;
    Vector3 dir = Vector3.zero;

    private bool _isFallGuysScene = false; // 폴가이즈 씬인지 확인

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        if (_owner.PhotonView.IsMine)
        {
            GameObject.FindWithTag("MainCamera").GetComponent<TPSCamera>().target = CameraRoot;
        }
    }

    // 키 입력과 이동방향 계산
    void Update()
    {
        if (!_owner.PhotonView.IsMine)
        {
            return;
        }
        GroundCheck();
        
        Jump();
    }

    private void FixedUpdate()
    {
        InputAndDir();
    }

    // 키 입력과 그에 따른 이동방향을 계산하는 함수
    void InputAndDir()
    {
        // 키 입력에 따라 방향 벡터 설정
        dir.x = Input.GetAxis("Horizontal");   // x축 방향 키 입력
        dir.z = Input.GetAxis("Vertical");     // z축 방향 키 입력
        Vector3 direction = new Vector3(dir.x, 0f, dir.z);
        float movementMagnitude = direction.magnitude;

        // 이동 애니메이션 설정
        _animator.SetFloat("Move", Mathf.Clamp01(movementMagnitude));

        // 기존 y축 속도를 유지하면서 새로운 방향으로 속도 설정
        rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z);

        if (dir != Vector3.zero)   // 키 입력이 있는 경우
        {
            // 카메라의 앞 방향을 기준으로 이동 방향 설정
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            direction = (forward.normalized * dir.z + Camera.main.transform.right * dir.x).normalized;

            // 이동 방향으로 캐릭터 회전
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f));

            // 걷기 애니메이션 설정
            _animator.SetBool("Walk", true);
        }
        else // 키 입력이 없는 경우
        {
            _animator.SetBool("Walk", false);
        }

        // 달리기 여부에 따라 이동 속도 및 애니메이션 설정
        if (_isFallGuysScene ||Input.GetKey(KeyCode.LeftShift))
        {
            rb.MovePosition(rb.position + direction * RunSpeed * Time.deltaTime);
            _isRunning = true;
            JumpPower = RunningJumpPower;
            _animator.SetBool("Run", true);
        }
        else
        {
            rb.MovePosition(rb.position + direction * Movespeed * Time.deltaTime);
            _isRunning = false;
            JumpPower = NormalJumpPower;
            _animator.SetBool("Run", false);
        }
    }

    // 점프 동작 구현
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)	// IsGrounded가 true일 때만 점프할 수 있도록
        {
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(JumpPower * -2f * Physics.gravity.y);
            rb.AddForce(jumpVelocity, ForceMode.VelocityChange);
            _animator.SetTrigger("Jump");
            Debug.Log($"{isGrounded}");
        }
    }

    // 땅에 있는지 검사하는 함수
    void GroundCheck()
    {
        RaycastHit hit;

        // 플레이어의 위치에서, 아래방향으로, groundDistance 만큼 ray를 쏴서, ground 레이어가 있는지 검사
        if (Physics.Raycast(LayerPoint.position, Vector3.down, out hit, groundDistance, ground))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
