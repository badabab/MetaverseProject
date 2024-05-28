using UnityEngine;
using Photon.Pun;

public class EndCollider : MonoBehaviour
{
    public Transform Start2, Start3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                if (gameObject.name == "End1")
                {
                    photonView.RPC("MovePlayer", RpcTarget.All, Start2.position);
                }
                else if (gameObject.name == "End2")
                {
                    photonView.RPC("MovePlayer", RpcTarget.All, Start3.position);
                }
                else if (gameObject.name == "End3")
                {
                    Debug.Log("게임 끝~");
                }
            }
        }
    }
}
