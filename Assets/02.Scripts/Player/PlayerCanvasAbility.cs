using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasAbility : PlayerAbility
{
    public static PlayerCanvasAbility Instance {  get; private set; }

    public Canvas PlayerCanvas;
    public Text NicknameTextUI;
    private PhotonView photonView;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        ShowMyNickname();
    }
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void ShowMyNickname()
    {
        NicknameTextUI.text = PhotonNetwork.NickName;
/*        if (photonView.IsMine)
        {
            string nickname = PlayerPrefs.GetString("LoggedInId");
            photonView.RPC("SetNickname", RpcTarget.AllBuffered, nickname);
        }*/
    }

    [PunRPC]
    public void SetNickname(string nickname)
    {
        NicknameTextUI.text = nickname;
    }
}
