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
    public bool isGrounded;
    public LayerMask LayerMask;
    public float groundDistance = 0.4f;
    public bool _isRunning;
    private bool _isJumping;
    private bool _isRunningJumping;
    public Transform LayerPoint;
    private Animator _animator;
    private bool _animationEnded;
    private Rigidbody rb;
    public Transform CameraRoot;
    private Vector3 dir = Vector3.zero;
    private CinemachineFreeLook cinemachineCamera;
    private ParticleSystem[] walkVFX;
    private int currentVFXIndex = 0;
    private float vfxTimer = 0;
    private bool _isFallGuysScene = false;
    private bool _isTowerClimbScene = false;
    private bool _isBattleTileScene = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
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

    void InputAndDir()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(dir.x, 0f, dir.z);
        float movementMagnitude = direction.magnitude;

        _animator.SetFloat("Move", Mathf.Clamp01(movementMagnitude));
        rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z);

        if (dir != Vector3.zero)
        {
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            direction = (forward.normalized * dir.z + Camera.main.transform.right * dir.x).normalized;
            var a = direction;
            a.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(a);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }

        direction.y = 0f;

        if (_isFallGuysScene || Input.GetKey(KeyCode.LeftShift))
        {
            Speed = RunSpeed;
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);
            _isRunning = true;
            _animator.SetBool("Run", true);
        }
        else
        {
            Speed = Movespeed;
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);
            _isRunning = false;
            _animator.SetBool("Run", false);
        }

        if (_isTowerClimbScene)
        {
            JumpPower = 6;
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                JumpCode();
            }
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded && !_isTowerClimbScene)
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
        if (photonView.IsMine)
        {
            PlayWalkVFX();
        }
        vfxTimer -= Time.deltaTime;
    }

    void JumpCounter()
    {
        if (isGrounded && JumpCount < 1)
        {
            JumpCount += 1;
        }
    }

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
