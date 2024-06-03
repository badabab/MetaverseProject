using Photon.Pun;
using UnityEngine;


//[RequireComponent(typeof(PlayerMoveAbility))]
//[RequireComponent(typeof(PlayerRotateAbility))]
[RequireComponent(typeof(PlayerGrabAbility))]
[RequireComponent(typeof(PlayerMovementAbility))]
public class Player : MonoBehaviour
{
    public PhotonView PhotonView {  get; private set; }
    
    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        if (PhotonView.IsMine)
        {
            UI_Minimap.Instance.MyPlayer = this;
        }
    }
}
