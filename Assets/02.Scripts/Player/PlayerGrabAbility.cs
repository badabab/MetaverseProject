using UnityEngine;
using Photon.Pun;

public class PlayerGrabAbility : MonoBehaviourPunCallbacks
{
    public float grabDuration = 4.0f; // 잡기 지속 시간
    public float grabberSpeedMultiplier = 0.7f; // 잡는 사람의 이동 속도 감소 비율
    public float grabbedSpeed = 0.5f; // 잡힌 사람의 이동 속도
    public float pushForce = 5.0f; // 잡기 상태가 풀릴 때 밀어내는 힘
    public LayerMask playerLayer; // 플레이어 레이어

    private GameObject grabbedPlayer = null;
    private Animator animator;
    private Rigidbody rb;
    private Rigidbody grabbedRb;
    private float grabTimer = 0.0f;
    private PlayerMoveAbility playerMoveAbility;
    private PlayerMoveAbility grabbedPlayerMoveAbility;
    private bool isGrabbed = false; // 잡힌 상태를 나타내는 플래그

    void Start()
    {
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        playerMoveAbility = GetComponent<PlayerMoveAbility>(); // PlayerMoveAbility 컴포넌트 가져오기
    }

    void Update()
    {
        if (!photonView.IsMine) // 이 클라이언트의 로컬 플레이어인지 확인
            return;

        if (Input.GetKeyDown(KeyCode.G)) // 'G' 키를 눌렀을 때
        {
            animator.SetBool("Grab", true); // Grab 애니메이션 실행
        }

        if (grabbedPlayer != null) // 잡힌 플레이어가 존재하면
        {
            grabTimer += Time.deltaTime; // 타이머 증가

            if (grabTimer >= grabDuration) // 4초가 지나면 놓기
            {
                ReleaseGrab();
            }
            else
            {
                // 잡고 있는 동안의 동작 처리
                grabbedPlayer.transform.position = transform.position + transform.forward * 1.5f; // 잡힌 플레이어의 위치 업데이트
                playerMoveAbility.Speed *= grabberSpeedMultiplier; // 잡는 사람의 이동 속도 감소
                grabbedPlayerMoveAbility.Speed = grabbedSpeed; // 잡힌 사람의 이동 속도 설정
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine || isGrabbed) // 이 클라이언트의 로컬 플레이어인지 확인하고, 잡힌 상태가 아닌지 확인
            return;

        // 현재 애니메이터 상태가 "Grab" 애니메이션인지 확인
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Grab") && other.CompareTag("Hand"))
        {
            Collider[] hitColliders = Physics.OverlapSphere(other.transform.position, 0.1f, playerLayer); // 손 콜라이더 범위 내의 플레이어 감지

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != gameObject)
                {
                    grabbedPlayer = hitCollider.gameObject; // 잡힌 플레이어 설정
                    grabbedRb = grabbedPlayer.GetComponent<Rigidbody>(); // 잡힌 플레이어의 Rigidbody 가져오기
                    grabbedPlayerMoveAbility = grabbedPlayer.GetComponent<PlayerMoveAbility>(); // 잡힌 플레이어의 PlayerMoveAbility 가져오기

                    if (grabbedPlayerMoveAbility != null)
                    {
                        grabbedPlayer.GetComponent<PhotonView>().RPC("OnGrabbed", RpcTarget.AllBuffered, photonView.ViewID); // RPC 호출하여 모든 클라이언트에 잡힌 상태 동기화
                        animator.SetBool("isGrabbing", true); // 잡기 애니메이션 설정
                        grabTimer = 0.0f; // 타이머 초기화
                        break;
                    }
                }
            }
        }
    }

    void ReleaseGrab()
    {
        if (grabbedPlayer != null) // 잡힌 플레이어가 존재하면
        {
            // 상대방을 밀어내는 힘을 가함
            Vector3 pushDirection = (grabbedPlayer.transform.position - transform.position).normalized; // 밀어내는 방향 계산
            grabbedRb.AddForce(pushDirection * pushForce, ForceMode.Impulse); // 밀어내는 힘 적용

            grabbedPlayer.GetComponent<PhotonView>().RPC("OnReleased", RpcTarget.AllBuffered); // RPC 호출하여 모든 클라이언트에 풀린 상태 동기화
            grabbedPlayer = null; // 잡힌 플레이어 초기화
            grabbedRb = null; // 잡힌 플레이어의 Rigidbody 초기화
            grabbedPlayerMoveAbility = null; // 잡힌 플레이어의 PlayerMoveAbility 초기화
            animator.SetBool("isGrabbing", false); // 잡기 애니메이션 해제
            animator.SetBool("Grab", false); // Grab 애니메이션 해제
            grabTimer = 0.0f; // 타이머 초기화
        }
    }

    [PunRPC]
    public void OnGrabbed(int viewID)
    {
        if (!photonView.IsMine) // 이 클라이언트의 로컬 플레이어인지 확인
            return;

        // 잡힌 상태 처리 (애니메이션 등)
        Debug.Log("Grabbed by player with ViewID: " + viewID); // 디버그 메시지 출력
        isGrabbed = true; // 잡힌 상태로 설정
    }

    [PunRPC]
    public void OnReleased()
    {
        if (!photonView.IsMine) // 이 클라이언트의 로컬 플레이어인지 확인
            return;

        // 풀린 상태 처리 (애니메이션 등)
        Debug.Log("Released"); // 디버그 메시지 출력
        isGrabbed = false; // 잡힌 상태 해제
    }
}
