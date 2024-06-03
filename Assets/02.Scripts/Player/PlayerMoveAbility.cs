using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveAbility : PlayerAbility
{
    public float Movespeed = 5f;
    public float RunSpeed = 15f;
    public float JumpPower = 3f;

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
        InputAndDir();
        Jump();

    }

    // 계산된 방향으로 물리적인 이동 구현
    private void FixedUpdate()
    {
       

        
        
    }

    // 키 입력과 그에 따른 이동방향을 계산하는 함수
    void InputAndDir()
    {
        dir.x = Input.GetAxis("Horizontal");   // x축 방향 키 입력
        dir.z = Input.GetAxis("Vertical");     // z축 방향 키 입력
        Vector3 direction = new Vector3(dir.x, 0f, dir.z);
        float movementMagnitude = direction.magnitude;

        _animator.SetFloat("Move", Mathf.Clamp01(movementMagnitude));

        rb.velocity =  new Vector3(direction.x,rb.velocity.y, direction.z);
        if (dir != Vector3.zero)   // 키입력이 존재하는 경우
        {
            transform.forward = dir;	// 키 입력 시, 입력된 방향으로 캐릭터의 방향을 바꿈
            _animator.SetBool("Walk", true);
        }
        else if (dir == Vector3.zero)
        {
            _animator.SetBool("Walk", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.MovePosition(rb.position + dir * RunSpeed * Time.deltaTime);
            _isRunning = true;
            _animator.SetBool("Run", true);
        }
        else
        {
            rb.MovePosition(rb.position + dir * Movespeed * Time.deltaTime);
            _isRunning = false;
            _animator.SetBool("Run", false);
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)	// IsGrounded가 true일 때만 점프할 수 있도록
        {
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(JumpPower * -2f * Physics.gravity.y);
            rb.AddForce(jumpVelocity, ForceMode.VelocityChange);
            _animator.SetTrigger("Jump");
            Debug.Log($"{isGrounded}");
        }
    }
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
