using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSpin : MonoBehaviour
{
    public GameObject Stage0;
    public GameObject Stage1;
    public GameObject Stage2;
    public GameObject Stage3;
    public GameObject Stage4;
    public GameObject Stage5;
    public GameObject UI_Spin;
    public TMP_Text StageNum;
    public float Spinspeed;

    private bool isRotating = false; // 회전 중인지 여부를 확인하기 위한 변수

    void Update()
    {
        // Update 함수 내에 회전 로직을 제거합니다.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRotating)
        {
            StartCoroutine(SpinUI());
        }
    }

    IEnumerator SpinUI()
    {
        isRotating = true; // 회전 시작

        float totalRotation = 0f;
        float rotationAmount = 360f; // 360도 회전
        float duration = 1f; // 회전에 걸리는 시간 (초 단위)
        float currentTime = 0f;

        while (currentTime < duration)
        {
            float rotationStep = (rotationAmount / duration) * Time.deltaTime;
            UI_Spin.transform.Rotate(0, 0, rotationStep);
            totalRotation += rotationStep;
            currentTime += Time.deltaTime;
            yield return null;
        }

        // 마지막으로 남은 차이만큼 회전 보정
        UI_Spin.transform.Rotate(0, 0, rotationAmount - totalRotation);

        isRotating = false; // 회전 종료
    }
}
