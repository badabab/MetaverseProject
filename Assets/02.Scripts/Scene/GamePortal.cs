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
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 20,
                IsVisible = true,
                IsOpen = true,
                EmptyRoomTtl = 1000 * 20,
            };


            if (gameObject.name == "BattleTilePortal")
            {
                PhotonManager.Instance.NextRoomName = "MiniGame1";
                //PhotonNetwork.JoinOrCreateRoom("MiniGame1", roomOptions, TypedLobby.Default);
            }
            else if (gameObject.name == "FallGuysPortal")
            {
                PhotonManager.Instance.NextRoomName = "MiniGame2";

            }
            else if (gameObject.name == "TowerClimbPortal")
            {
                PhotonManager.Instance.NextRoomName = "MiniGame3";

            }
           
           // PhotonNetwork.LeaveRoom();

        }
    }
}
