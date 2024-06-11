using Photon.Pun;
using UnityEngine;

public class GamePortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        if (other.CompareTag("Player") && photonView.IsMine)
        {
            if (gameObject.name == "BattleTilePortal")
            {
                PhotonNetwork.LoadLevel("BattleTileScene");
                PhotonNetwork.Destroy(other.gameObject); // 로컬 플레이어 객체 제거
            }
            else if (gameObject.name == "FallGuysPortal")
            {
                PhotonNetwork.LoadLevel("FallGuysScene");
                PhotonNetwork.Destroy(other.gameObject); // 로컬 플레이어 객체 제거
            }
            else if (gameObject.name == "TowerClimbPortal")
            {
                PhotonNetwork.LoadLevel("TowerClimbScene");
                PhotonNetwork.Destroy(other.gameObject); // 로컬 플레이어 객체 제거
            }
        }
    }
}
