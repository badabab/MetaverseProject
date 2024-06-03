using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinMovement : MonoBehaviour
{
    public GameObject pumpkin; // 플레이어 태그를 갖고 있는 오브젝트
    public float targetYSelected = 500f; // 선택된 오브젝트가 이동할 Y 좌표
    public float smoothSpeed = 0.1f; // 이동할 때 사용할 부드러운 이동 속도
    public float timer = 5f; // 이동 후 대기할 시간

    private Vector3 velocity = Vector3.zero;
    private bool isMoving = false;

    void Start()
    {

    }

    IEnumerator MoveToY(GameObject obj, Vector3 targetPosition, float smoothSpeed)
    {
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.01f)
        {
            obj.transform.position = Vector3.SmoothDamp(obj.transform.position, targetPosition, ref velocity, smoothSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator DisableAndEnablePumpkin()
    {
        pumpkin.SetActive(false);
        yield return new WaitForSeconds(timer);
        pumpkin.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌");

            // 0 또는 1 중 랜덤하게 선택
            int randomValue = Random.Range(0, 2);

            if (randomValue == 0)
            {
                StartCoroutine(DisableAndEnablePumpkin());
            }
            else
            {
                StartCoroutine(MoveToY(pumpkin, new Vector3(pumpkin.transform.position.x, targetYSelected, pumpkin.transform.position.z), smoothSpeed));
            }
        }
    }
}