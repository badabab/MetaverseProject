using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PhotonView PhotonView {  get; private set; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }
}
