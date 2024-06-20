using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UI_Information : PlayerAbility
{
    public TextMeshProUGUI Nickname;
    public Image X;
    public Image Y;

    void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetNickname(PhotonNetwork.NickName);
            SetXY();
        }
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

    public void SetNickname(string nickname)
    {
        Nickname.text = nickname;
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Nickname") && targetPlayer == _owner.photonView.Owner)
        {
            SetNickname((string)changedProps["Nickname"]);
        }
        if (changedProps.ContainsKey("CharacterIndex"))
        {
            SetXY();
        }
    }
}
