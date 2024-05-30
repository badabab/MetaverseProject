using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Information : MonoBehaviour
{
    public Text Nickname;
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

    private void SetNickname()
    {
        string loggedInUsername = PlayerPrefs.GetString("LoggedInUser", "");
        if (!string.IsNullOrEmpty(loggedInUsername))
        {
            Nickname.text = loggedInUsername;
        }
    }
}
