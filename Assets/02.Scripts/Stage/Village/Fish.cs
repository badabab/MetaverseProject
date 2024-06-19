using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{ 
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        // 자신의 클라이언트에서만 실행되도록 설정
        if (photonView.IsMine)
        {
            // 여기에 자신의 클라이언트에서 수행해야 할 초기화 작업을 추가할 수 있습니다.
        }
    }

    // 예를 들어, 특정 상태를 RPC로 모든 클라이언트에게 동기화할 수 있습니다.
    [PunRPC]
    void SetFishState(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}

