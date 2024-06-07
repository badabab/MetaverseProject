using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TunnelObject : MonoBehaviourPun
{
    public Transform[] points; // 각 터널 객체에 대한 포인트 배열
    public float transitionDuration = 0.1f; // 각 구간 이동 시간

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                StartCoroutine(TeleportPlayer(other.transform, photonView));
            }
        }
    }

    private IEnumerator TeleportPlayer(Transform playerTransform, PhotonView photonView)
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

        // 플레이어의 최종 위치를 다른 클라이언트에 동기화
        photonView.RPC("UpdatePlayerPosition", RpcTarget.All, playerTransform.position);
    }

    [PunRPC]
    void UpdatePlayerPosition(Vector3 newPosition)
    {
        Transform playerTransform = PhotonView.Find(photonView.ViewID).transform;
        if (playerTransform != null && !photonView.IsMine)
        {
            playerTransform.position = newPosition;
        }
    }
}
