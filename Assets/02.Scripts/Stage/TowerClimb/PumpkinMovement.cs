using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinMovement : MonoBehaviour
{
    public Transform pumpkin; // 플레이어 태그를 갖고 있는 오브젝트
    public float targetYSelected; // 선택된 오브젝트가 이동할 Y 좌표
    public float moveSpeed; // 이동 속도

    private bool isMoving = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            Debug.Log("충돌");

            // 2분의 1 확률로 선택
            if (Random.value < 0.5f)
            {
                isMoving = true;
                StartCoroutine(MovePumpkin());
            }
            else
            {
                StartCoroutine(DisableAndEnableObject());
            }
        }
    }

    IEnumerator MovePumpkin()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = pumpkin.position;
        Vector3 targetPosition = new Vector3(pumpkin.position.x, targetYSelected, pumpkin.position.z);

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;
            pumpkin.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            yield return null;
        }

        isMoving = false;
    }

    IEnumerator DisableAndEnableObject()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(true);
    }
}