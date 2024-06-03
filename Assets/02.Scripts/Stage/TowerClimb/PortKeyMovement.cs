using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortKeyMovement : MonoBehaviour
{
    [SerializeField]
    public GameObject PortKey1;
    public GameObject PortKey2;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌");
            // 충돌한 오브젝트가 플레이어 태그를 가지고 있다면
            Vector3 newPosition = PortKey2.transform.position; // PortKey2의 위치를 가져옴
            other.transform.position = newPosition; // 충돌한 오브젝트의 위치를 PortKey2의 위치로 순간 이동
        }
    }
}