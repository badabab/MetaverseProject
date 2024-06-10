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
       ShowMyNickname();
    }
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void ShowMyNickname()
    {
        PhotonView photonView = GetComponentInParent<PhotonView>();
        SetNickname(photonView.Owner.NickName);
    }
    [PunRPC]
    public void SetNickname(string nickname)
    {
        NicknameTextUI.text = nickname;
    }

}
