using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerCanvasAbility : MonoBehaviourPunCallbacks
{
    public static PlayerCanvasAbility Instance {  get; private set; }

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
       //ShowMyNickname();
    }
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void ShowMyNickname()
    {
        PhotonView photonView = GetComponentInParent<PhotonView>();
        SetNickname(photonView.Owner.NickName);

        return;

        /*if (photonView != null)
        {
            Debug.Log("PhotonView found.");
            if (photonView.IsMine)
            {
                Debug.Log("PhotonView is mine.");
                string nickname = PlayerPrefs.GetString("LoggedInId");
                photonView.RPC("SetNickname", RpcTarget.AllBuffered, nickname);
            }
            else
            {
                Debug.LogError("PhotonView is not owned by this player.");
            }
        }
        else
        {
            Debug.LogError("PhotonView not found.");
        }*/
    }
    [PunRPC]
    public void SetNickname(string nickname)
    {
        NicknameTextUI.text = nickname;
    }

}
