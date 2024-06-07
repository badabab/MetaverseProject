using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelObject : MonoBehaviour
{
    public Transform[] points; // 각 터널 객체에 대한 포인트 배열
    public float transitionDuration = 0.1f; // 각 구간 이동 시간

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    private IEnumerator TeleportPlayer(Transform playerTransform)
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 startPosition = points[i].position;
            Vector3 endPosition = points[i + 1].position;
            float elapsedTime = 0f;

            while (elapsedTime < transitionDuration)
            {
                playerTransform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / transitionDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            playerTransform.position = endPosition;
        }
    }
}
