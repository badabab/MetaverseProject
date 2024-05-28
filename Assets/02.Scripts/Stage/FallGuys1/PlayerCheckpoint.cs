using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCheckpoint : MonoBehaviourPunCallbacks
{
    private Vector3 currentCheckpoint;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "FallGuysScene")
        {
            this.enabled = false; // 씬이 "FallGuysScene"이 아니면 스크립트를 비활성화
            return;
        }

        currentCheckpoint = new Vector3(0, 5.5f, 110); // 초기 체크포인트 설정
    }

    [PunRPC]
    public void UpdateCheckpoint(Vector3 checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            PhotonView photonView = GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                Vector3 newCheckpoint = other.transform.position;
                photonView.RPC("UpdateCheckpoint", RpcTarget.All, newCheckpoint);
            }
        }
        else if (other.CompareTag("Respawn"))
        {
            PhotonView photonView = GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                transform.position = currentCheckpoint;
            }
        }
    }
}
