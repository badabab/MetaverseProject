using Photon.Pun;
using UnityEngine;

public class Checkpoint : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                Vector3 checkpoint = transform.position;
                photonView.RPC("UpdateCheckpoint", RpcTarget.All, checkpoint);
            }
        }
    }
}
