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
        // 커스텀프로퍼티 없이 사용할 수 있는 코드
        SetNickname(_owner.photonView.Owner.NickName);


        return;

        /*if (_owner.photonView.IsMine) // 사용하지 않는 코드
        {
            SetNickname(PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            SetNickname(_owner.photonView.Owner.NickName);
        }*/
    }

    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void SetNickname(string nickname)
    {
        NicknameTextUI.text = nickname;
    }

    // 커스텀 프로퍼티는 사용하지 않으나 이렇게 사용해도 실행은 됨
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Nickname") && targetPlayer == _owner.photonView.Owner)
        {
            //SetNickname((string)changedProps["Nickname"]);
        }
    }
}
