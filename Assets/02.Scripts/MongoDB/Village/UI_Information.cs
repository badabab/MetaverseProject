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
        int Index = PersonalManager.Instance.CheckCharacterIndex();
        if (Index >= 1 && Index <= 13)
        {
            X.gameObject.SetActive(true);
            Y.gameObject.SetActive(false);
        }
        else if (Index <= 14 && 26 >= Index)
        {
            X.gameObject.SetActive(false);
            Y.gameObject.SetActive(true);
        }
    }

    public void SetNickname()
    {
        string nickname = PlayerPrefs.GetString("LoggedInId");
        Nickname.text = nickname;
        Debug.Log($"{nickname}");
    }
}
