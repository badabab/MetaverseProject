using Photon.Pun;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveAbility : PlayerAbility
{
    public float Speed;
    private float Movespeed = 3f;
    private float RunSpeed = 5f;

    
    private float NormalJumpPower= 2;
    private float RunningJumpPower= 4;

    public int JumpCount;
    private int MaxJumpCount = 1;

    private float JumpPower;

    public bool isGrounded;		// 땅에 서있는지 체크하기 위한 bool값
    public LayerMask LayerMask;	// 레이어마스크 설정
    public float groundDistance = 0.4f;		// Ray를 쏴서 검사하는 거리

    private bool _isRunning;

    private bool _isJumping;
    private bool _isRunningJumping;

    public Transform LayerPoint;
    private Animator _animator;
    private bool _animationEnded;

    Rigidbody rb;
    public Transform CameraRoot;
    Vector3 dir = Vector3.zero;

    private bool _isFallGuysScene = false; // 폴가이즈 씬인지 확인
    private bool _isTowerClimbScene = false;
    void Start()
    {
        if (!_owner.PhotonView.IsMine)
        {
            return;
        }

        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        if (_owner.PhotonView.IsMine)
        {
            GameObject.FindWithTag("MainCamera").GetComponent<TPSCamera>().target = CameraRoot;
        }
        _isFallGuysScene = SceneManager.GetActiveScene().name == "FallGuysScene";
        _isTowerClimbScene = SceneManager.GetActiveScene().name == "TowerClimbScene";

        Physics.gravity = new Vector3(0, -9.81f, 0);
    }

    // 키 입력과 이동방향 계산
    void Update()
    {
        if (!_owner.PhotonView.IsMine)
        {
            return;
        }
        GroundCheck();
        JumpCounter();

        if (Input.GetKeyDown(KeyCode.T))
        {
            _animator.SetTrigger("Punching");
        }

        if (JumpCount >= MaxJumpCount)
        {
            JumpCount = MaxJumpCount;
        }
    }

    private void FixedUpdate()
    {
        if ( !_owner.PhotonView.IsMine)
        {
            return;
        }
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
           // _animator.SetBool("Walk", true);
        }
        else // 키 입력이 없는 경우
        {
            //_animator.SetBool("Walk", false);
        }

        // 달리기 여부에 따라 이동 속도 및 애니메이션 설정
        if (_isFallGuysScene ||Input.GetKey(KeyCode.LeftShift))
        {
          
            Speed = RunSpeed;
            
            rb.MovePosition(rb.position + direction * Speed * Time.deltaTime);
            _isRunning = true;
            
            _animator.SetBool("Run", true);
        }
        else
        {
            Speed = Movespeed;
            
            rb.MovePosition(rb.position + direction * Speed * Time.deltaTime);
            _isRunning = false;
            
            _animator.SetBool("Run", false);
        }
        if (_isTowerClimbScene)
        {
            JumpPower = 20;
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                JumpCount -= 1;
                rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
                _animator.SetBool("Jump", true);
            }

        }
        if (Input.GetKey(KeyCode.Space) && isGrounded && !_isTowerClimbScene) 	// IsGrounded가 true일 때만 점프할 수 있도록
        {
            if (_isRunning)
            {
                JumpPower = RunningJumpPower;
            }
            else
            {
                JumpPower = NormalJumpPower;
            }
            JumpCount -= 1;
            rb.AddForce((Vector3.up * JumpPower)/2f, ForceMode.Impulse);
            _animator.SetBool("Jump",true);

            
        }


    }

    // 점프 동작 구현
    void JumpCounter()
    {

        if (isGrounded && JumpCount<1)
        {
            JumpCount += 1;
            // 추가 동작 구현
        }
    }

    // 땅에 있는지 검사하는 함수
    void GroundCheck()
    {
        RaycastHit hit;
        // Default 레이어만 포함된 레이어 마스크 생성
        int defaultLayerMask = LayerMask.GetMask("Default");

        // 플레이어의 위치에서, 아래방향으로, groundDistance 만큼 ray를 쏴서, Default 레이어가 있는지 검사
        if (Physics.Raycast(LayerPoint.position, Vector3.down, out hit, groundDistance, defaultLayerMask))
        {
            isGrounded = true;
            
            Physics.gravity = new Vector3(0, -9.81f, 0);

        }
        else
        {
            isGrounded = false;
            _animator.SetBool("Jump", false);
        }
    }
}

