using Cinemachine;
using JetBrains.Annotations;
using Photon.Pun;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMoveAbility : PlayerAbility
{
    public float Speed;
    private float Movespeed = 3f; // 걷기 속도
    private float RunSpeed = 5f; // 달리기 속도

    private float NormalJumpPower = 2; // 일반 점프 힘
    private float RunningJumpPower = 4; // 달리기 점프 힘

    public int JumpCount; // 현재 점프 횟수
    private int MaxJumpCount = 1; // 최대 점프 횟수

    private float _JumpPower;

    public bool isGrounded;        // 땅에 있는지 확인하기 위한 변수
    public LayerMask LayerMask;    // 레이어 마스크 설정
    public float groundDistance = 0.4f;    // 땅을 확인할 거리

    public bool _isRunning; // 달리고 있는지 확인하기 위한 변수

    private bool _isJumping; // 점프 중인지 확인하기 위한 변수

    public Transform LayerPoint; // 레이어 포인트 (땅을 확인할 위치)
    private Animator _animator;
    private bool _animationEnded; // 애니메이션이 끝났는지 확인하기 위한 변수

    Rigidbody rb;
    public Transform CameraRoot;
    Vector3 dir = Vector3.zero;

    private CinemachineFreeLook cinemachineCamera;

    private ParticleSystem[] walkVFX; // 걷기 VFX 배열
    private int currentVFXIndex = 0; // 현재 걷기 VFX 인덱스
    private float vfxTimer = 0; // VFX 타이머

    private bool _isFallGuysScene = false; // FallGuysScene인지 확인
    private bool _isTowerClimbScene = false; // TowerClimbScene인지 확인
    private bool _isBattleTileScene = false; // BattleTileScene인지 확인

    private string _sceneName; // 현재 씬 이름

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _sceneName = SceneManager.GetActiveScene().name;

        _isFallGuysScene = _sceneName == "FallGuysScene";
        _isTowerClimbScene = _sceneName == "TowerClimbScene";
        _isBattleTileScene = _sceneName == "BattleTileScene";

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

    // 키 입력과 이동 방향 계산
    void Update()
    {
        if (!_owner.PhotonView.IsMine)
        {
            return;
        }

        GroundCheck();
        JumpCounter();

        if (JumpCount >= MaxJumpCount)
        {
            JumpCount = MaxJumpCount;
        }

        if (isGrounded)
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

        if (_sceneName == "BattleTileWinScene")
        {
            transform.position = new Vector3(0, 10.5f, -63);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (_sceneName == "FallGuysWinScene")
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (_sceneName == "TowerClimbWinScene")
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (_sceneName.EndsWith("WinScene"))
        {
            return;
        }
        if (_isFallGuysScene)
        {
            if (FallGuysManager.Instance._currentGameState == GameState.Loading)
            {
                return;
            }
        }
        else if (_isBattleTileScene)
        {
            if (BattleTileManager.Instance.CurrentGameState == GameState.Loading)
            {
                return;
            }
        }
        InputAndDir();
    }

    // 키 입력과 그에 따른 이동 방향을 계산하는 함수
    void InputAndDir()
    {
        dir.x = Input.GetAxis("Horizontal");   // x축 방향 입력
        dir.z = Input.GetAxis("Vertical");     // z축 방향 입력
        Vector3 direction = new Vector3(dir.x, 0f, dir.z);
        float movementMagnitude = direction.magnitude;

        _animator.SetFloat("Move", Mathf.Clamp01(movementMagnitude));
        rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z);

        if (dir != Vector3.zero)   // 키 입력이 있는 경우
        {
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            direction = (forward.normalized * dir.z + Camera.main.transform.right * dir.x).normalized;

            var a = direction;
            a.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(a);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f));

            _animator.SetBool("Walk", true);
            SoundManager.instance.StopSfx(SoundManager.Sfx.PlayerWalking);
        }
        else // 키 입력이 없는 경우
        {
            _animator.SetBool("Walk", false);
            SoundManager.instance.StopSfx(SoundManager.Sfx.PlayerWalking);
        }

        direction.y = 0f;

        // 달리는지 여부에 따라 이동 속도 및 애니메이션 설정
        if (_isFallGuysScene || Input.GetKey(KeyCode.LeftShift))
        {
            Speed = RunSpeed * 2;
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);
            _isRunning = true;
            _animator.SetBool("Run", true);
        }
        else
        {
            Speed = Movespeed * 2;
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);
            _isRunning = false;
            _animator.SetBool("Run", false);
        }

        if (PhotonNetwork.CurrentRoom.Name == "MiniGame1")
        {
            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Max(-8f, Mathf.Min(8f, newPosition.x));
            newPosition.z = Mathf.Max(-0.6f, Mathf.Min(14f, newPosition.z));
            transform.position = newPosition;
        }

        if (_isTowerClimbScene)
        {
            _JumpPower = 6;
            if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.V)) && isGrounded)
            {
                JumpCode();
            }
        }
        else if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.V)) && isGrounded)   // 땅에 있을 때만 점프
        {
            if (_isBattleTileScene)
            {
                return;
            }
            if (_isRunning)
            {
                _JumpPower = RunningJumpPower;
            }
            else
            {
                _JumpPower = NormalJumpPower;
            }
            JumpCount -= 1;

            JumpCode();
        }
    }

    public void Jump(float jumpPower)
    {
        rb.AddForce((Vector3.up * jumpPower) / 2f, ForceMode.Impulse);
        _animator.SetBool("Jump", true);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.PlayerJump);
        if (photonView.IsMine)
        {
            PlayWalkVFX();
        }
        vfxTimer -= Time.deltaTime;
    }

    private void JumpCode()
    {
        rb.AddForce((Vector3.up * _JumpPower) / 2f, ForceMode.Impulse);
        _animator.SetBool("Jump", true);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.PlayerJump); // 점프 사운드 재생

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

            vfxTimer = 1; // 타이머 재설정
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