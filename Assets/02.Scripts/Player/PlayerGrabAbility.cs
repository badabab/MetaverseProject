using UnityEngine;
using Photon.Pun;

public class PlayerGrabAbility : MonoBehaviourPunCallbacks
{
    public float grabRange = 2.0f; // 잡을 수 있는 최대 거리
    public LayerMask playerLayer; // 플레이어 레이어

    private GameObject grabbedPlayer = null;

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.G)) // 'G' 키를 눌러서 잡기 시도
        {
            TryGrab();
        }

        if (Input.GetKeyUp(KeyCode.G)) // 'G' 키를 떼면 놓기
        {
            ReleaseGrab();
        }

        if (grabbedPlayer != null)
        {
            // 잡고 있는 플레이어의 위치를 계속 업데이트
            grabbedPlayer.transform.position = transform.position + transform.forward * 1.5f;
        }
    }

    void TryGrab()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, grabRange, playerLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject)
            {
                grabbedPlayer = hitCollider.gameObject;
                grabbedPlayer.GetComponent<PhotonView>().RPC("OnGrabbed", RpcTarget.AllBuffered, photonView.ViewID);
                break;
            }
        }
    }

    void ReleaseGrab()
    {
        if (grabbedPlayer != null)
        {
            grabbedPlayer.GetComponent<PhotonView>().RPC("OnReleased", RpcTarget.AllBuffered);
            grabbedPlayer = null;
        }
    }

    [PunRPC]
    public void OnGrabbed(int viewID)
    {
        if (!photonView.IsMine)
            return;

        // 잡힌 상태 처리 (애니메이션 등)
        Debug.Log("Grabbed by player with ViewID: " + viewID);
    }

    [PunRPC]
    public void OnReleased()
    {
        if (!photonView.IsMine)
            return;

        // 풀린 상태 처리 (애니메이션 등)
        Debug.Log("Released");
    }
}
 