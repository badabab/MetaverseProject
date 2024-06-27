using Photon.Pun;
using UnityEngine;

public class PlayerAnimatorSync : MonoBehaviourPun, IPunObservable
{
    private Animator animator;
    private PhotonAnimatorView photonAnimatorView;

    // 애니메이션 파라미터 변수 선언
    private float move;
    private bool run;
    private bool runJump;
    private bool walk;
    private bool jump;
    private bool win;
    private bool sad;
    private bool attack;
    private bool attack2; // 추가
    private bool flyingAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        photonAnimatorView = GetComponent<PhotonAnimatorView>();

        // 파라미터 동기화 설정
        photonAnimatorView.SetParameterSynchronized("Move", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimatorView.SetParameterSynchronized("Run", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimatorView.SetParameterSynchronized("RunJump", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimatorView.SetParameterSynchronized("Walk", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimatorView.SetParameterSynchronized("Jump", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimatorView.SetParameterSynchronized("Win", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimatorView.SetParameterSynchronized("Sad", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimatorView.SetParameterSynchronized("Attack", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimatorView.SetParameterSynchronized("Attack2", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete); // 추가
        photonAnimatorView.SetParameterSynchronized("FlyingAttack", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // 로컬 플레이어의 입력 처리 및 애니메이션 파라미터 업데이트
            HandleInput();
        }
        else
        {
            // 원격 플레이어의 애니메이션 파라미터를 업데이트
            animator.SetFloat("Move", move);
            animator.SetBool("Run", run);
            animator.SetBool("RunJump", runJump);
            animator.SetBool("Walk", walk);
            animator.SetBool("Jump", jump);
            animator.SetBool("Win", win);
            animator.SetBool("Sad", sad);
            animator.SetBool("Attack", attack);
            animator.SetBool("Attack2", attack2); // 추가
            animator.SetBool("FlyingAttack", flyingAttack);
        }
    }

    private void HandleInput()
    {
        // 예시 입력 처리
        move = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        animator.SetFloat("Move", move);
        animator.SetBool("Walk", move != 0 || horizontal != 0);

        if (Input.GetMouseButtonDown(1))
        {
            if (animator.GetCurrentAnimatorStateInfo(3).IsName("Attack"))
            {
                attack = false;
                attack2 = true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(3).IsName("Attack2"))
            {
                attack2 = false;
                attack = true;
            }
            else
            {
                attack = true;
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            attack = false;
            attack2 = false;
        }

        animator.SetBool("Attack", attack);
        animator.SetBool("Attack2", attack2);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            jump = false;
        }

        animator.SetBool("Jump", jump);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }

        animator.SetBool("Run", run);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 플레이어의 데이터를 전송
            stream.SendNext(animator.GetFloat("Move"));
            stream.SendNext(animator.GetBool("Run"));
            stream.SendNext(animator.GetBool("RunJump"));
            stream.SendNext(animator.GetBool("Walk"));
            stream.SendNext(animator.GetBool("Jump"));
            stream.SendNext(animator.GetBool("Win"));
            stream.SendNext(animator.GetBool("Sad"));
            stream.SendNext(animator.GetBool("Attack"));
            stream.SendNext(animator.GetBool("Attack2")); // 추가
            stream.SendNext(animator.GetBool("FlyingAttack"));
        }
        else
        {
            // 원격 플레이어의 데이터를 수신
            move = (float)stream.ReceiveNext();
            run = (bool)stream.ReceiveNext();
            runJump = (bool)stream.ReceiveNext();
            walk = (bool)stream.ReceiveNext();
            jump = (bool)stream.ReceiveNext();
            win = (bool)stream.ReceiveNext();
            sad = (bool)stream.ReceiveNext();
            attack = (bool)stream.ReceiveNext();
            attack2 = (bool)stream.ReceiveNext(); // 추가
            flyingAttack = (bool)stream.ReceiveNext();
        }
    }
}
