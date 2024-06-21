using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerAttackAbility : MonoBehaviourPunCallbacks
{
    public float pushForce = 5f; // 밀리는 힘의 크기
    public LayerMask playerLayer; // 플레이어 레이어
    public Animator animator; // 애니메이터 컴포넌트
    private PhotonAnimatorView photonAnimatorView; // PhotonAnimatorView 컴포넌트 참조 추가
    private Collider punchCollider; // 주먹 콜라이더
    private bool isAttacking = false; // 공격 중인지 여부
    private PlayerMoveAbility playerMoveAbility; // 플레이어 이동 능력

    void Start()
    {
        playerMoveAbility = GetComponent<PlayerMoveAbility>(); // PlayerMoveAbility 컴포넌트 가져오기
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        photonAnimatorView = GetComponent<PhotonAnimatorView>(); // PhotonAnimatorView 컴포넌트 가져오기
        punchCollider = GetComponentInChildren<Collider>(); // 주먹에 있는 Collider 가져오기

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
        if (playerMoveAbility._isRunning)
        {
            animator.SetBool("FlyingAttack", true);
        }
        else
        {
            animator.SetBool("Attack", true); // 공격 애니메이션 실행
        }
        
        photonAnimatorView.SetParameterSynchronized("Attack", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete); // 동기화 설정
        isAttacking = true; // 공격 중 상태로 설정

        if (punchCollider != null && punchCollider.CompareTag("Hand")) // 주먹 콜라이더가 존재하며 Hand 태그가 있는지 확인
        {
            punchCollider.enabled = true; // 공격 시 주먹 콜라이더 활성화
        }

        StartCoroutine(DisablePunchColliderAfterDelay(0.7f)); // 0.7초 후에 콜라이더 비활성화
    }

    IEnumerator DisablePunchColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기

        if (punchCollider != null && punchCollider.CompareTag("Hand")) // 주먹 콜라이더가 존재하며 Hand 태그가 있는지 확인
        {
            punchCollider.enabled = false; // 주먹 콜라이더 비활성화
        }
        animator.SetBool("Attack", false); // 공격 애니메이션 해제
        animator.SetBool("FlyingAttack", false);
        isAttacking = false; // 공격 중 상태 해제
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            PhotonView otherPhotonView = other.GetComponentInParent<PhotonView>();

            if (otherPhotonView == null)
            {
                Debug.LogWarning("PhotonView not found on " + other.gameObject.name); // 디버그 경고 로그 추가
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !otherPhotonView.IsMine)
            {
                if (playerMoveAbility._isRunning)
                {
                    pushForce = 6f;
                }
                else
                {
                    pushForce = 2f;
                }

                Debug.Log("Collision detected with " + other.gameObject.name); // 디버그 로그 추가
                Vector3 pushDirection = (other.transform.position - transform.position).normalized; // 밀리는 방향 계산
                otherPhotonView.RPC("ApplyPushForce", RpcTarget.AllBuffered, pushDirection, pushForce); // 상대 플레이어를 밀리게 하는 RPC 호출
                Debug.Log("때림");
            }
        }
    }

    [PunRPC]
    public void ApplyPushForce(Vector3 pushDirection, float force)
    {
        StartCoroutine(ApplyPushForceCoroutine(pushDirection, force));
    }

    private IEnumerator ApplyPushForceCoroutine(Vector3 pushDirection, float force)
    {
        Rigidbody targetRigidbody = GetComponentInParent<Rigidbody>();

        if (targetRigidbody != null)
        {
            float duration = 0.5f;
            float elapsedTime = 0f;
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = initialPosition + pushDirection * force;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                targetRigidbody.MovePosition(Vector3.Lerp(initialPosition, targetPosition, t)); // 부드럽게 이동
                yield return null;
            }
        }
    }
}
