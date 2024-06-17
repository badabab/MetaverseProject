using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAbility : MonoBehaviourPunCallbacks
{
    public float attackRange = 2.0f;
    public int attackDamage = 1;
    public LayerMask playerLayer;
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
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
                targetPhotonView.RPC("TakeDamage", RpcTarget.AllBuffered, attackDamage);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        GetComponent<PlayerHealth>().ApplyDamage(damage);
    }
}
