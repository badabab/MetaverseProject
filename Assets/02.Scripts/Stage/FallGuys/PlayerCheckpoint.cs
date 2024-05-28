using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCheckpoint : MonoBehaviourPunCallbacks
{
    private Vector3 _currentCheckpoint;

    private void Start()
    {
        if (!photonView.IsMine) return;

        if (SceneManager.GetActiveScene().name != "FallGuysScene")
        {
            this.enabled = false; // 씬이 "FallGuysScene"이 아니면 스크립트를 비활성화
            return;
        }

        _currentCheckpoint = new Vector3(0, 5.5f, 110); // 초기 체크포인트 설정
        transform.position = _currentCheckpoint;
    }

    [PunRPC]
    public void UpdateCheckpoint(Vector3 checkpoint)
    {
        _currentCheckpoint = checkpoint;
    }
    [PunRPC]
    public void MovePlayer(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        if (other.CompareTag("Checkpoint"))
        {
            Vector3 newCheckpoint = other.transform.position;
            photonView.RPC("UpdateCheckpoint", RpcTarget.All, newCheckpoint);
        }
        else if (other.gameObject.name == "Respawn")
        {
            transform.position = _currentCheckpoint;
        }
    }
}
