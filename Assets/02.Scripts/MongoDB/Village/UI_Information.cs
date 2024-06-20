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
        Debug.Log("OnPlayerPropertiesUpdate called");

        if (changedProps == null)
        {
            Debug.LogError("changedProps is null");
            return;
        }

        if (targetPlayer == null)
        {
            Debug.LogError("targetPlayer is null");
            return;
        }

        if (_owner == null)
        {
            Debug.LogError("_owner is null");
            return;
        }

        if (_owner.photonView == null)
        {
            Debug.LogError("_owner.photonView is null");
            return;
        }

        if (changedProps.ContainsKey("Nickname"))
        {
            Debug.Log("changedProps contains 'Nickname'");
            if (targetPlayer == _owner.photonView.Owner)
            {
                Debug.Log("targetPlayer is _owner.photonView.Owner");
                SetNickname((string)changedProps["Nickname"]);
            }
            else
            {
                Debug.LogWarning("targetPlayer is not _owner.photonView.Owner");
            }
        }
        else
        {
            Debug.LogWarning("changedProps does not contain 'Nickname'");
        }
    }
}
