using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinMovement : MonoBehaviour
{
    public List<GameObject> objectAList; // 첫 번째 오브젝트 리스트
    public List<GameObject> objectBList; // 두 번째 오브젝트 리스트
    public string playerTag = "Player"; // 플레이어 태그
    public float speed = 5f; // 이동 속도
    private List<bool> isMovingA; // 오브젝트 A가 움직이는지 여부 리스트
    private List<bool> isMovingB; // 오브젝트 B가 움직이는지 여부 리스트

    private void Start()
    {
        // 초기화
        isMovingA = new List<bool>(new bool[objectAList.Count]);
        isMovingB = new List<bool>(new bool[objectBList.Count]);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 플레이어 태그를 가진 경우
        if (collision.gameObject.CompareTag(playerTag))
        {
            StartCoroutine(DelayedMove());
        }
    }

    private IEnumerator DelayedMove()
    {
        // 1초 지연
        yield return new WaitForSeconds(1f);

        // 5대5 랜덤으로 동작
        for (int i = 0; i < objectAList.Count; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (!isMovingA[i]) StartCoroutine(MoveObjectA(i));
            }
            else
            {
                if (!isMovingB[i]) StartCoroutine(MoveObjectB(i));
            }
        }
    }

    private IEnumerator MoveObjectA(int index)
    {
        isMovingA[index] = true;

        GameObject objectA = objectAList[index];

        // 오브젝트 A를 Y=600까지 이동
        while (objectA.transform.position.y < 600f)
        {
            objectA.transform.position += Vector3.up * speed * Time.deltaTime;
            yield return null;
        }

        // 2초간 정지
        yield return new WaitForSeconds(2f);

        // 오브젝트 A 비활성화
        objectA.SetActive(false);
        isMovingA[index] = false;
    }

    private IEnumerator MoveObjectB(int index)
    {
        isMovingB[index] = true;

        GameObject objectB = objectBList[index];

        // 오브젝트 B를 Y=300까지 이동
        while (objectB.transform.position.y < 300f)
        {
            objectB.transform.position += Vector3.up * speed * Time.deltaTime;
            yield return null;
        }

        // 2초간 정지
        yield return new WaitForSeconds(2f);

        // 오브젝트 B를 Y=800까지 이동
        while (objectB.transform.position.y < 800f)
        {
            objectB.transform.position += Vector3.up * speed * Time.deltaTime;
            yield return null;
        }

        isMovingB[index] = false;
    }
}
