using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTile : MonoBehaviour
{
    public GameObject GameObject1; // 첫 번째 게임 오브젝트
    public GameObject GameObject2; // 두 번째 게임 오브젝트

    private GameObject fakeGlassPane; // 가짜 유리판

    void Start()
    {
        // 두 게임 오브젝트 중에서 가짜 유리판을 랜덤으로 선택
        if (Random.Range(0, 2) == 0)
        {
            fakeGlassPane = GameObject1;
        }
        else
        {
            fakeGlassPane = GameObject2;
        }
    }

    // 충돌 감지
    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 객체가 플레이어 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 충돌한 객체와 fakeGlassPane로 결정된 객체가 충돌했을 때, fakeGlassPane를 비활성화
            if (collision.gameObject == fakeGlassPane)
            {
                fakeGlassPane.SetActive(false);
            }
        }
    }
}