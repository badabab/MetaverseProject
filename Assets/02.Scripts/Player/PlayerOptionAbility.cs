using Photon.Pun;
using UnityEngine;

public class PlayerOptionAbility : PlayerAbility
{
    public void Pause()
    {
        if (!_owner.photonView.IsMine) return;
        photonView.RPC("RPC_Pause", RpcTarget.AllBuffered, _owner.photonView.IsMine);
    }

    public void Continue()
    {
        if (!_owner.photonView.IsMine) return;
        photonView.RPC("RPC_Continue", RpcTarget.AllBuffered, _owner.photonView.IsMine);
    }

    [PunRPC]
    private void RPC_Pause(PhotonView photonView)
    {
        if (photonView != null && photonView.IsMine)
        {
            if (photonView.gameObject.TryGetComponent<PlayerMoveAbility>(out var moveAbility))
            {
                moveAbility.enabled = false;
            }
        }
    }
    [PunRPC]
    private void RPC_Continue(PhotonView photonView)
    {
        if (photonView != null && photonView.IsMine)
        {
            if (photonView.gameObject.TryGetComponent<PlayerMoveAbility>(out var moveAbility))
            {
                moveAbility.enabled = true;
            }
        }
    }
}
