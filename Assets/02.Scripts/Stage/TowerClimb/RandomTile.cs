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

    void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그를 가진 오브젝트가 가짜 유리판에 닿았을 때
        if (other.CompareTag("Player") && this.gameObject == fakeGlassPane)
        {
            // 가짜 유리판 비활성화
            fakeGlassPane.SetActive(false);
        }
    }
}