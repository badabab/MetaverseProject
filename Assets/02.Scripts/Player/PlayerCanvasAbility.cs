using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasAbility : PlayerAbility
{
    public static PlayerCanvasAbility Instance {  get; private set; }

    public Canvas PlayerCanvas;
    public Text NicknameTextUI;
    private Player player;

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
        player = GetComponentInParent<Player>();
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
        PhotonView photonView = GetComponentInParent(typeof(PhotonView)) as PhotonView;
        //NicknameTextUI.text = PhotonNetwork.NickName;
        if (photonView.IsMine)
        {
            string nickname = PlayerPrefs.GetString("LoggedInId");
            photonView.RPC("SetNickname", RpcTarget.AllBuffered, nickname);
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
