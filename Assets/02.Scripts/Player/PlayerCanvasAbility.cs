using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerCanvasAbility : MonoBehaviourPunCallbacks
{
    public static PlayerCanvasAbility Instance { get; private set; }

    public Canvas PlayerCanvas;
    public TextMeshProUGUI NicknameTextUI;

    private new void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PhotonView photonview = GetComponentInParent<PhotonView>();
        if (photonview.IsMine)
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
