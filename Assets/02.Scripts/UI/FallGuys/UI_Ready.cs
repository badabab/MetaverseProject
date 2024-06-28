using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class UI_Ready : MonoBehaviourPunCallbacks
{
    public GameObject ReadyDescriptionUI;
    public GameObject ReadyUI;
    public GameObject NotReady;
    public GameObject Ready;

    void Update()
    {
        if (FallGuysManager.Instance._currentGameState == GameState.Ready)
        {
            ReadyDescriptionUI.SetActive(true);
            ReadyUI.gameObject.SetActive(true);
        }
        else if (FallGuysManager.Instance._currentGameState == GameState.Loading)
        {
            ReadyDescriptionUI.SetActive(false);
            ReadyUI.gameObject.SetActive(false);
        }
        CheakReadyButton();
    }

    void CheakReadyButton()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsReady", out object isReady))
        {
            bool isReadyValue = (bool)isReady;
            Debug.Log("IsReady: " + isReadyValue);
            Ready.gameObject.SetActive(isReadyValue);
        }
    }
}
