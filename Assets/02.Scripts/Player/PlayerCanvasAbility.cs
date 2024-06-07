using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasAbility : PlayerAbility
{
    public static PlayerCanvasAbility Instance {  get; private set; }

    public Canvas PlayerCanvas;
    public Text NicknameTextUI;
    private Player player;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        player = GetComponent<Player>();

        ShowMyNickname();
    }
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void ShowMyNickname()
    {
        //NicknameTextUI.text = PhotonNetwork.NickName;
        if (player.PhotonView.IsMine)
        {
            string nickname = PlayerPrefs.GetString("LoggedInId");
            player.PhotonView.RPC("SetNickname", RpcTarget.AllBuffered, nickname);
            Debug.Log(nickname);
         }
    }

    [PunRPC]
    public void SetNickname()
    {
        NicknameTextUI.text = PhotonNetwork.NickName;
        Debug.Log($"{PhotonNetwork.NickName}");
    }
}
