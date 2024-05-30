using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Information : MonoBehaviourPunCallbacks
{
    public TMP_Text Nickname;
    public Image X;
    public Image Y;

    void Start()
    {
        SetNickname();
        SetXY();
    }

    private void SetXY()
    {
        if (UI_Lobby.SelectedType == PlayerType.Female)
        {
            X.gameObject.SetActive(true);
            Y.gameObject.SetActive(false);
        }
        else if (UI_Lobby.SelectedType == PlayerType.Male)
        {
            X.gameObject.SetActive(false);
            Y.gameObject.SetActive(true);
        }
    }

    [PunRPC]
    public void SetNickname()
    {
        Nickname.text = PhotonNetwork.NickName;
        Debug.Log($"{PhotonNetwork.NickName}");
    }
}
