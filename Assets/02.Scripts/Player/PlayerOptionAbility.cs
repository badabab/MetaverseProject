using Photon.Pun;
using UnityEngine;

public class PlayerOptionAbility : PlayerAbility
{
    public void Pause()
    {
        if (!_owner.photonView.IsMine) return;
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        if (!_owner.photonView.IsMine) return;
        Time.timeScale = 1f;
    }

    [PunRPC]
    public void TeleportToVillage()
    {
        if (!_owner.photonView.IsMine) return;
        PhotonNetwork.LoadLevel("VillageScene");
    }
}
