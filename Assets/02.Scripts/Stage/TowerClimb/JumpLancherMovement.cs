using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLancherMovement : MonoBehaviour
{
    public float jumpForce = 10f; // 플레이어를 점프시킬 힘의 크기

    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 플레이어인 경우
        if (other.CompareTag("Player"))
        {
            // 플레이어의 Rigidbody 컴포넌트를 가져옴
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();

            // 플레이어의 Rigidbody가 존재하는 경우에만 작업 수행
            if (playerRigidbody != null)
            {
                // 플레이어를 위쪽 방향으로 점프시킴
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
