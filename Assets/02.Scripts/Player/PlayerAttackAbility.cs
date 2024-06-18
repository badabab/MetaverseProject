using Photon.Pun; // Photon.Pun 네임스페이스 사용
using System.Collections; // System.Collections 네임스페이스 사용
using System.Collections.Generic; // System.Collections.Generic 네임스페이스 사용
using UnityEngine; // UnityEngine 네임스페이스 사용

public class PlayerAttackAbility : MonoBehaviourPunCallbacks // Photon.Pun의 MonoBehaviourPunCallbacks를 상속받은 클래스 선언
{
    public float pushForce = 5f; // 밀리는 힘의 크기
    public LayerMask playerLayer; // 플레이어 레이어
    public Animator animator; // 애니메이터 컴포넌트

    private Collider punchCollider; // 주먹 콜라이더
    private bool isAttacking = false; // 공격 중인지 여부

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        punchCollider = GetComponentInChildren<Collider>(); // 주먹에 있는 BoxCollider 가져오기
        if (punchCollider != null && punchCollider.CompareTag("Hand")) // 주먹 콜라이더가 존재하며 Hand 태그가 있는지 확인
        {
            punchCollider.isTrigger = true; // 콜라이더를 트리거로 설정하여 충돌 감지
            punchCollider.enabled = false; // 초기에는 비활성화
        }
    }

    void Update()
    {
        if (!photonView.IsMine) // 로컬 플레이어인지 확인
            return;

        if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼 클릭으로 공격
        {
            Attack(); // 공격 함수 호출
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); // 공격 애니메이션 실행
        isAttacking = true; // 공격 중 상태로 설정
        if (punchCollider != null && punchCollider.CompareTag("Hand")) // 주먹 콜라이더가 존재하며 Hand 태그가 있는지 확인
        {
            punchCollider.enabled = true; // 공격 시 주먹 콜라이더 활성화
        }
        StartCoroutine(DisablePunchColliderAfterDelay(0.5f)); // 0.5초 후에 콜라이더 비활성화
    }

    IEnumerator DisablePunchColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        if (punchCollider != null && punchCollider.CompareTag("Hand")) // 주먹 콜라이더가 존재하며 Hand 태그가 있는지 확인
        {
            punchCollider.enabled = false; // 주먹 콜라이더 비활성화
        }
        isAttacking = false; // 공격 중 상태 해제
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.gameObject.layer == LayerMask.NameToLayer("Player") && !other.GetComponent<PhotonView>().IsMine)
        {
            // 공격 중이며 충돌한 객체가 플레이어 레이어에 속하고 로컬 플레이어가 아닌 경우
            PhotonView targetPhotonView = other.GetComponent<PhotonView>(); // 충돌한 객체의 PhotonView 컴포넌트를 가져옴
            if (targetPhotonView != null) // PhotonView가 존재하는지 확인
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized; // 밀리는 방향 계산
                targetPhotonView.RPC("ApplyPushForce", RpcTarget.AllBuffered, pushDirection, pushForce); // 상대 플레이어를 밀리게 하는 RPC 호출
            }
        }
    }

    [PunRPC]
    public void ApplyPushForce(Vector3 pushDirection, float force)
    {
        Rigidbody targetRigidbody = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        if (targetRigidbody != null) // Rigidbody가 존재하는지 확인
        {
            targetRigidbody.AddForce(pushDirection * force, ForceMode.Impulse); // 힘을 가해 밀기
        }
    }
}
