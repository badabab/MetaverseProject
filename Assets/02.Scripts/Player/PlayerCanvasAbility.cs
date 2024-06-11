using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerCanvasAbility : PlayerAbility
{
    public Canvas PlayerCanvas;
    public TextMeshProUGUI NicknameTextUI;

    private void Start()
    {
        if (_owner.photonView.IsMine)
        {
            SetNickname(PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            SetNickname(_owner.photonView.Owner.NickName);
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

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Nickname") && targetPlayer == _owner.photonView.Owner)
        {
            SetNickname((string)changedProps["Nickname"]);
        }
    }
}
