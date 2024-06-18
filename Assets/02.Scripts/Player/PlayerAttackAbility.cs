using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAbility : MonoBehaviourPunCallbacks
{
    public float attackRange = 2.0f; // 공격 범위
    public int attackDamage = 1; // 공격 데미지 (사용되지 않음)
    public float pushForce = 5f; // 밀리는 힘의 크기
    public LayerMask playerLayer; // 공격 대상 레이어
    public Animator animator; // 애니메이터 컴포넌트

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }

    void Update()
    {
        if (!photonView.IsMine) // 로컬 플레이어인지 확인
            return;

        if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼 클릭으로 공격
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); // 공격 애니메이션 실행

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, playerLayer)) // 공격 범위 내에 있는 플레이어 감지
        {
            PhotonView targetPhotonView = hit.collider.GetComponent<PhotonView>();
            if (targetPhotonView != null && !targetPhotonView.IsMine) // 자기 자신이 아닌 다른 플레이어인지 확인
            {
                targetPhotonView.RPC("ApplyPushForce", RpcTarget.AllBuffered, transform.forward, pushForce);
            }
        }
    }

    [PunRPC]
    public void ApplyPushForce(Vector3 attackerForward, float force)
    {
        Rigidbody targetRigidbody = GetComponent<Rigidbody>();
        if (targetRigidbody != null)
        {
            // 밀리는 방향 계산
            Vector3 pushDirection = attackerForward;
            targetRigidbody.AddForce(pushDirection * force, ForceMode.Impulse); // 힘을 가해 밀기
        }
    }
}
