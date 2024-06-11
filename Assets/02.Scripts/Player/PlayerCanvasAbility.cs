using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerCanvasAbility : MonoBehaviourPunCallbacks
{
    public Canvas PlayerCanvas;
    public TextMeshProUGUI NicknameTextUI;

    private void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetNickname", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
    }
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    [PunRPC]
    public void SetNickname(string nickname)
    {
        NicknameTextUI.text = nickname;
    }
}
