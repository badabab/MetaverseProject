using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

[RequireComponent(typeof(PlayerMoveAbility))]
//[RequireComponent(typeof(PlayerRotateAbility))]
[RequireComponent(typeof(PlayerGrabAbility))]
//[RequireComponent(typeof(PlayerMovementAbility))]


public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PhotonView { get; private set; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        if (PhotonView.IsMine)
        {
            UI_Minimap.Instance.MyPlayer = this;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
