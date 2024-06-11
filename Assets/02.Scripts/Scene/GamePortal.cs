using Photon.Pun;
using UnityEngine;

public class GamePortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
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
