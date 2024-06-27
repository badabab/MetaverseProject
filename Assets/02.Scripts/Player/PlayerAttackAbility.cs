using Photon.Pun; // Photon.Pun 네임스페이스 사용
using System.Collections; // System.Collections 네임스페이스 사용
using UnityEngine; // UnityEngine 네임스페이스 사용

public class PlayerAttackAbility : MonoBehaviourPunCallbacks // Photon.Pun의 MonoBehaviourPunCallbacks를 상속받은 클래스 선언
{
    private float pushForce; // 밀리는 힘의 크기
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
            if(playerMoveAbility._isRunning == true)
            {
                animator.SetBool("FlyingAttack", true);
            }

            else if(playerMoveAbility._isRunning == false )
            {
                if (animator.GetCurrentAnimatorStateInfo(4).IsName("Attack"))
                {
                    animator.SetBool("Attack", false);
                    animator.SetBool("Attack2", true);
                    photonAnimatorView.SetParameterSynchronized("Attack2", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
                }
                else if (animator.GetCurrentAnimatorStateInfo(4).IsName("Attack2"))
                {
                    animator.SetBool("Attack2", false);
                    animator.SetBool("Attack", true);
                    photonAnimatorView.SetParameterSynchronized("Attack", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
                }
                else
                {
                    animator.SetBool("Attack", true);
                    photonAnimatorView.SetParameterSynchronized("Attack", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
                }
            }
           
            isAttacking = true; // 공격 중 상태로 설정

            if (punchCollider != null && punchCollider.CompareTag("Hand")) // 주먹 콜라이더가 존재하며 Hand 태그가 있는지 확인
            {
                punchCollider.enabled = true; // 공격 시 주먹 콜라이더 활성화
            }

            StartCoroutine(DisablePunchColliderAfterDelay(0.5f)); // 0.5초 후에 콜라이더 비활성화
        }
    }

    IEnumerator DisablePunchColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기

        if (punchCollider != null && punchCollider.CompareTag("Hand")) // 주먹 콜라이더가 존재하며 Hand 태그가 있는지 확인
        {
            punchCollider.enabled = false; // 주먹 콜라이더 비활성화
        }
        animator.SetBool("Attack", false); // 기본 공격 애니메이션 해제
        animator.SetBool("Attack2", false);
        animator.SetBool("FlyingAttack", false);
        isAttacking = false; // 공격 중 상태 해제
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking) // 공격 중인지 확인
        {
            PhotonView otherPhotonView = other.GetComponentInParent<PhotonView>(); // 충돌한 객체의 PhotonView를 가져옴

            if (otherPhotonView == null) // PhotonView가 존재하지 않으면
            {
                return; // 함수 종료
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !otherPhotonView.IsMine) // 충돌한 객체가 플레이어 레이어에 속하고 로컬 플레이어가 아니면
            {
                if (playerMoveAbility._isRunning) // 플레이어가 달리는 중인지 확인
                {
                    pushForce = 4f; // 달리는 중일 때의 밀어내는 힘 설정
                }
                else if (animator.GetCurrentAnimatorStateInfo(4).IsName("Attack2")) // Attack2 애니메이션이 실행 중인지 확인
                {
                    pushForce = 2.5f; // Attack2의 밀어내는 힘 설정
                }
                else
                {
                    pushForce = 1.5f; // 기본 밀어내는 힘 설정
                }

                Vector3 pushDirection = (other.transform.position - transform.position).normalized; // 밀리는 방향 계산
                //pushDirection.y = 0;
                otherPhotonView.RPC("ApplyPushForce", RpcTarget.AllBuffered, pushDirection, pushForce); // 상대 플레이어를 밀리게 하는 RPC 호출
            }
        }
    }

    [PunRPC]
    public void ApplyPushForce(Vector3 pushDirection, float force)
    {
        StartCoroutine(ApplyPushForceCoroutine(pushDirection, force)); // 힘 적용 코루틴 시작
    }

    private IEnumerator ApplyPushForceCoroutine(Vector3 pushDirection, float force)
    {
        Rigidbody targetRigidbody = GetComponentInParent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기

        if (targetRigidbody != null) // Rigidbody가 존재하는지 확인
        {
            float duration = 0.5f; // 밀리는 지속 시간
            float elapsedTime = 0f; // 경과 시간 초기화
            Vector3 initialPosition = transform.position; // 초기 위치 저장
            Vector3 targetPosition = initialPosition + pushDirection * force; // 목표 위치 계산

            while (elapsedTime < duration) // 경과 시간이 지속 시간보다 적을 동안
            {
                elapsedTime += Time.deltaTime; // 경과 시간 증가
                float t = elapsedTime / duration; // 진행 비율 계산
                targetRigidbody.MovePosition(Vector3.Lerp(initialPosition, targetPosition, t)); // 부드럽게 이동
                yield return null; // 다음 프레임까지 대기
            }
        }
    }
}
