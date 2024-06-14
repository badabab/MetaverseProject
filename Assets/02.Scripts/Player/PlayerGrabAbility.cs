using Photon.Pun;
using UnityEngine;

public class PlayerGrabAbility : MonoBehaviourPunCallbacks
{
    public float grabDuration = 4.0f; // 잡기 지속 시간
    public float grabberSpeedMultiplier = 0.7f; // 잡는 사람의 이동 속도 감소 비율
    public float grabbedSpeed = 0.5f; // 잡힌 사람의 이동 속도
    public float pushForce = 5.0f; // 잡기 상태가 풀릴 때 밀어내는 힘
    public LayerMask playerLayer; // 플레이어 레이어
    public Transform handTransform; // 손 콜라이더의 Transform

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
            TryGrab();
        }

        if (grabbedPlayer != null) // 잡힌 플레이어가 존재하면
        {
            grabTimer += Time.deltaTime; // 타이머 증가

            if (grabTimer >= grabDuration || Input.GetKeyUp(KeyCode.G)) // 4초가 지나거나 'G' 키를 떼면 놓기
            {
                ReleaseGrab();
            }
        }
    }

    void TryGrab()
    {
        Collider[] hitColliders = Physics.OverlapSphere(handTransform.position, 0.5f, playerLayer); // 손 콜라이더 범위 내의 플레이어 감지

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
                    grabbedPlayer.transform.SetParent(handTransform); // 손 콜라이더의 자식으로 설정하여 끌고 다닐 수 있게 함
                    grabTimer = 0.0f; // 타이머 초기화
                    break;
                }
            }
        }
    }

    void ReleaseGrab()
    {
        if (grabbedPlayer != null) // 잡힌 플레이어가 존재하면
        {
            grabbedPlayer.transform.SetParent(null); // 부모 관계 해제

            // 상대방을 밀어내는 힘을 가함
            Vector3 pushDirection = (grabbedPlayer.transform.position - transform.position).normalized; // 밀어내는 방향 계산
            grabbedRb.AddForce(pushDirection * pushForce, ForceMode.Impulse); // 밀어내는 힘 적용

            grabbedPlayer.GetComponent<PhotonView>().RPC("OnReleased", RpcTarget.AllBuffered); // RPC 호출하여 모든 클라이언트에 풀린 상태 동기화
            grabbedPlayer = null; // 잡힌 플레이어 초기화
            grabbedRb = null; // 잡힌 플레이어의 Rigidbody 초기화
            grabbedPlayerMoveAbility = null; // 잡힌 플레이어의 PlayerMoveAbility 초기화
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
        isGrabbed = true; // 잡힌 상태로 설정
    }

    [PunRPC]
    public void OnReleased()
    {
        if (!photonView.IsMine) // 이 클라이언트의 로컬 플레이어인지 확인
            return;

        // 풀린 상태 처리 (애니메이션 등)
        isGrabbed = false; // 잡힌 상태 해제
    }
}
