using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveAbility : PlayerAbility, IPunObservable
{
    public float Speed;
    private float Movespeed = 3f;
    private float RunSpeed = 5f;

    private float NormalJumpPower = 2;
    private float RunningJumpPower = 4;

    public int JumpCount;
    private int MaxJumpCount = 1;

    private float JumpPower;

    public bool isGrounded; // 땅에 서있는지 체크하기 위한 bool값
    public LayerMask LayerMask; // 레이어마스크 설정
    public float groundDistance = 0.4f; // Ray를 쏴서 검사하는 거리

    public bool _isRunning;

    private bool _isJumping;
    private bool _isRunningJumping;

    public Transform LayerPoint;
    private Animator _animator;
    private PhotonAnimatorView photonAnimatorView; // PhotonAnimatorView 컴포넌트
    private bool _animationEnded;

    Rigidbody rb;
    public Transform CameraRoot;
    Vector3 dir = Vector3.zero;

    private CinemachineFreeLook cinemachineCamera;

    //public ParticleSystem WalkVFX;
    //public ParticleSystem JumpVFX;
    private ParticleSystem[] walkVFX; // Walk VFX 배열
    private int currentVFXIndex = 0; // 현재 재생 중인 Walk VFX 인덱스
    private float vfxTimer = 0;

    private bool _isFallGuysScene = false; // 폴가이즈 씬인지 확인
    private bool _isTowerClimbScene = false;
    private bool _isBattleTileScene = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        photonAnimatorView = GetComponent<PhotonAnimatorView>(); // PhotonAnimatorView 컴포넌트 가져오기

        _isFallGuysScene = SceneManager.GetActiveScene().name == "FallGuysScene";
        _isTowerClimbScene = SceneManager.GetActiveScene().name == "TowerClimbScene";
        _isBattleTileScene = SceneManager.GetActiveScene().name == "BattleTileScene";

        if (_owner.PhotonView.IsMine && !_isTowerClimbScene && !_isBattleTileScene)
        {
            GameObject mainCamera = GameObject.FindWithTag("MainCamera");
            if (mainCamera != null)
            {
                TPSCamera tpsCamera = mainCamera.GetComponent<TPSCamera>();
                if (tpsCamera != null)
                {
                    tpsCamera.target = CameraRoot;
                }
            }
        }

        ParticleSystem[] allParticleSystems = GetComponentsInChildren<ParticleSystem>();
        walkVFX = System.Array.FindAll(allParticleSystems, ps => ps.gameObject.name.StartsWith("Walk"));
        for (int i = 0; i < walkVFX.Length; i++)
        {
            walkVFX[i].gameObject.SetActive(false);
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
        JumpCounter();

        if (Input.GetKeyDown(KeyCode.T))
        {
            _animator.SetTrigger("Punching");
            photonAnimatorView.SetParameterSynchronized("Punching", PhotonAnimatorView.ParameterType.Trigger, PhotonAnimatorView.SynchronizeType.Discrete);
        }

        if (JumpCount >= MaxJumpCount)
        {
            JumpCount = MaxJumpCount;
        }

        if (this.isGrounded)
        {
            vfxTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!_owner.PhotonView.IsMine)
        {
            return;
        }
        if (_isFallGuysScene)
        {
            if (FallGuysManager.Instance._currentGameState == GameState.Loading)
            { return; }
        }
        else if (_isBattleTileScene)
        {
            if (BattleTileManager.Instance.CurrentGameState == GameState.Loading)
            { return; }
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
        photonAnimatorView.SetParameterSynchronized("Move", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Continuous);

        // 기존 y축 속도를 유지하면서 새로운 방향으로 속도 설정
        rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z);

        if (dir != Vector3.zero)   // 키 입력이 있는 경우
        {
            // 카메라의 앞 방향을 기준으로 이동 방향 설정
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            direction = (forward.normalized * dir.z + Camera.main.transform.right * dir.x).normalized;

            var a = direction;
            a.y = 0f;
            // 이동 방향으로 캐릭터 회전
            Quaternion targetRotation = Quaternion.LookRotation(a);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }

        direction.y = 0f;

        // 달리기 여부에 따라 이동 속도 및 애니메이션 설정
        if (_isFallGuysScene || Input.GetKey(KeyCode.LeftShift))
        {
            Speed = RunSpeed;
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);
            _isRunning = true;
            _animator.SetBool("Run", true);
            photonAnimatorView.SetParameterSynchronized("Run", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        }
        else
        {
            Speed = Movespeed;
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);
            _isRunning = false;
            _animator.SetBool("Run", false);
            photonAnimatorView.SetParameterSynchronized("Run", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        }

        if (_isTowerClimbScene)
        {
            JumpPower = 6;
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                JumpCode();
            }
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded && !_isTowerClimbScene)  // IsGrounded가 true일 때만 점프할 수 있도록
        {
            if (_isBattleTileScene)
            { return; }
            if (_isRunning)
            {
                JumpPower = RunningJumpPower;
            }
            else
            {
                JumpPower = NormalJumpPower;
            }
            JumpCount -= 1;
            JumpCode();
        }
    }

    void JumpCode()
    {
        rb.AddForce((Vector3.up * JumpPower) / 2f, ForceMode.Impulse);
        _animator.SetBool("Jump", true);
        photonAnimatorView.SetParameterSynchronized("Jump", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);

        if (photonView.IsMine)
        {
            PlayWalkVFX();
        }
        vfxTimer -= Time.deltaTime;
    }

    // 점프 동작 구현
    void JumpCounter()
    {
        if (isGrounded && JumpCount < 1)
        {
            JumpCount += 1;
        }
    }

    // 땅에 있는지 검사하는 함수
    void GroundCheck()
    {
        RaycastHit hit;
        int defaultLayerMask = LayerMask.GetMask("Default");

        if (Physics.Raycast(LayerPoint.position, Vector3.down, out hit, groundDistance, defaultLayerMask))
        {
            isGrounded = true;
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
        else
        {
            isGrounded = false;
            _animator.SetBool("Jump", false);
            photonAnimatorView.SetParameterSynchronized("Jump", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        }
    }

    void PlayWalkVFX()
    {
        if (walkVFX.Length == 0) return;

        if (vfxTimer <= 0)
        {
            if (currentVFXIndex >= 0 && currentVFXIndex < walkVFX.Length)
            {
                walkVFX[currentVFXIndex].gameObject.SetActive(false);
            }

            currentVFXIndex = (currentVFXIndex + 1) % walkVFX.Length;
            walkVFX[currentVFXIndex].gameObject.SetActive(true);

            photonView.RPC("ActivateVFX", RpcTarget.Others, currentVFXIndex);

            vfxTimer = 1;
        }
        else
        {
            vfxTimer -= Time.deltaTime;
        }
    }

    [PunRPC]
    void ActivateVFX(int vfxIndex)
    {
        if (vfxIndex >= 0 && vfxIndex < walkVFX.Length)
        {
            foreach (var vfx in walkVFX)
            {
                vfx.gameObject.SetActive(false);
            }

            walkVFX[vfxIndex].gameObject.SetActive(true);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(vfxTimer);
            stream.SendNext(currentVFXIndex);
        }
        else
        {
            vfxTimer = (float)stream.ReceiveNext();
            currentVFXIndex = (int)stream.ReceiveNext();
        }
    }
}
