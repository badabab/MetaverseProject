using Photon.Pun;
using UnityEngine;

public class PlayerAnimatorSync : MonoBehaviourPun, IPunObservable
{
    private Animator animator;

    // 애니메이션 파라미터 변수 선언
    private float speed;
    private float direction;
    private bool isAttacking;
    private bool isGrabbing;
    private bool isJumping;
    private bool isRunning;
    private bool isRunningAttack;
    private void Awake()
    {
        animator = GetComponent<Animator>();
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
            animator.SetFloat("Speed", speed);
            animator.SetFloat("Direction", direction);
            animator.SetBool("Attack", isAttacking);
            animator.SetBool("isGrabbing", isGrabbing);
            animator.SetBool("Jump", isJumping);
            animator.SetBool("FlyingAttack", isRunningAttack);
        }
    }

    private void HandleInput()
    {
        // 예시 입력 처리
        speed = Input.GetAxis("Vertical");
        direction = Input.GetAxis("Horizontal");

        // 애니메이터 파라미터 업데이트
        animator.SetFloat("Speed", speed);
        animator.SetFloat("Direction", direction);

        if ( Input.GetKey(KeyCode.LeftShift) )
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        animator.SetBool("Run", isRunning);

        if(Input.GetMouseButtonDown(1)&& isRunning)
        {
            isRunningAttack = true;
        }
        else
        {
            isRunningAttack = false;
        }
        animator.SetBool("FlyingAttack", isRunningAttack);
        // 공격과 관련된 입력 처리
        if (Input.GetMouseButtonDown(1))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        animator.SetBool("Attack", isAttacking);

        // 잡기와 관련된 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            isGrabbing = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isGrabbing = false;
        }

        animator.SetBool("isGrabbing", isGrabbing);

        // 점프와 관련된 입력 처리
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        animator.SetBool("Jump", isJumping);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 플레이어의 데이터를 전송
            stream.SendNext(animator.GetFloat("Speed"));
            stream.SendNext(animator.GetFloat("Direction"));
            stream.SendNext(animator.GetFloat("Run"));
            stream.SendNext(animator.GetBool("Attack"));
            stream.SendNext(animator.GetBool("isGrabbing"));
            stream.SendNext(animator.GetBool("Jump"));
            stream.SendNext(animator.GetBool("FlyingAttack"));
            
        }
        else
        {
            // 원격 플레이어의 데이터를 수신
            speed = (float)stream.ReceiveNext();
            direction = (float)stream.ReceiveNext();
            isAttacking = (bool)stream.ReceiveNext();
            isGrabbing = (bool)stream.ReceiveNext();
            isJumping = (bool)stream.ReceiveNext();
            isRunning = (bool)stream.ReceiveNext();
            isRunningAttack = (bool)stream.ReceiveNext();
        }
    }
}
