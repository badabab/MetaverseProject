using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOptionAbility : PlayerAbility
{
    public void Pause()
    {
        if (!_owner.photonView.IsMine) return;
        if (SceneManager.GetActiveScene().name == "TowerClimbScene")
        {
            photonView.RPC("RPC_RotationStop", RpcTarget.AllBuffered, photonView.ViewID);
        }
        else
        {
            photonView.RPC("RPC_Pause", RpcTarget.AllBuffered, photonView.ViewID);
        }
        _owner.photonView.GetComponent<Animator>().SetFloat("Move", 0f);
    }

    public void Continue()
    {
        if (!_owner.photonView.IsMine) return;
        if (SceneManager.GetActiveScene().name == "TowerClimbScene")
        {
            photonView.RPC("RPC_RotationPlay", RpcTarget.AllBuffered, photonView.ViewID);
        }
        else
        {
            photonView.RPC("RPC_Continue", RpcTarget.AllBuffered, photonView.ViewID);
        }
    }
    [PunRPC]
    public void RPC_RotationStop(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null && targetView.IsMine)
        {
            if (targetView.gameObject.TryGetComponent<PlayerRotateAbility>(out var rotationAbility))
            {
                rotationAbility.enabled = false;
                Debug.Log($"{rotationAbility}");
            }
        }
    }
    [PunRPC]
    public void PRC_RotationPlay(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null && targetView.IsMine)
        {
            if (targetView.gameObject.TryGetComponent<PlayerRotateAbility>(out var rotationAbility))
            {
                rotationAbility.enabled = false;
                Debug.Log($"{rotationAbility}");
            }
        }
    }

    [PunRPC]
    private void RPC_Pause(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null && targetView.IsMine)
        {
            if (targetView.gameObject.TryGetComponent<PlayerMoveAbility>(out var moveAbility))
            {
                moveAbility.enabled = false;
                Debug.Log($"{moveAbility}");
            }
        }
    }

    [PunRPC]
    private void RPC_Continue(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null && targetView.IsMine)
        {
            if (targetView.gameObject.TryGetComponent<PlayerMoveAbility>(out var moveAbility))
            {
                moveAbility.enabled = true;
            }
        }
    }
}
