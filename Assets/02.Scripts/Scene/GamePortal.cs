using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
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
                PhotonManager.Instance.LeaveAndLoadRoom("MiniGame1");
                Debug.Log("{MiniGame1}");
            }
            else if (gameObject.name == "FallGuysPortal")
            {
                PhotonManager.Instance.LeaveAndLoadRoom("MiniGame2");
                Debug.Log("{MiniGame2}");
            }
            else if (gameObject.name == "TowerClimbPortal")
            {
                PhotonManager.Instance.LeaveAndLoadRoom("MiniGame3");
                Debug.Log("{MiniGame3}");
            }
            Debug.Log("방떠남");
        }
    }

}
