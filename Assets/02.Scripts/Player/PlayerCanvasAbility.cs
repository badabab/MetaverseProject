using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerCanvasAbility : PlayerAbility
{
    public Canvas PlayerCanvas;
    public TextMeshProUGUI NicknameTextUI;

    private void Start()
    {
        if (_owner.photonView.IsMine)
        {
            SetNickname(PhotonNetwork.NickName);
        }
    }

    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void SetNickname(string nickname)
    {
        NicknameTextUI.text = nickname;
    }
}
