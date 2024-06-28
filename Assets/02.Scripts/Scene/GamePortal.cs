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
            SoundManager.instance.PlaySfx(SoundManager.Sfx.VillagePortal);
            if (gameObject.name == "BattleTilePortal")
            {
                PhotonManager.Instance.LeaveAndLoadRoom("MiniGame1");
                //PhotonManager.Instance.NextRoomName = "MiniGame1";
                //PhotonNetwork.JoinOrCreateRoom("MiniGame1", roomOptions, TypedLobby.Default);
            }
            else if (gameObject.name == "FallGuysPortal")
            {
                PhotonManager.Instance.LeaveAndLoadRoom("MiniGame2");
                //PhotonManager.Instance.NextRoomName = "MiniGame2";
                //PhotonNetwork.JoinOrCreateRoom("MiniGame2", roomOptions, TypedLobby.Default);

            }
            else if (gameObject.name == "TowerClimbPortal")
            {
                PhotonManager.Instance.LeaveAndLoadRoom("MiniGame3");
                //PhotonManager.Instance.NextRoomName = "MiniGame3";
                //PhotonNetwork.JoinOrCreateRoom("MiniGame3", roomOptions, TypedLobby.Default);
            }
            SoundManager.instance.StopSfx(SoundManager.Sfx.VillagePortal);
            //PhotonNetwork.LeaveRoom();
        }
    }
}
