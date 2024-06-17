using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public int maxHealth = 10;
    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        if (!photonView.IsMine) // 로컬 플레이어인지 확인
            return;

        currentHealth -= damage;
        Debug.Log($"Player {photonView.ViewID} took {damage} damage, current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"Player {photonView.ViewID} died");
        // 사망 처리 로직 (예: 리스폰, 게임 오버 등)
        // 이 예제에서는 단순히 로그를 출력합니다.
    }
}
