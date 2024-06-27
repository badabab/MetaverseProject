using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UI_ReadyPeople : MonoBehaviour
{
    public TextMeshProUGUI TotalPeople;
    public TextMeshProUGUI ReadyPeople;

    void Update()
    {
        // 전체 플레이어 목록을 얻어옴
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        int readyCount = 0;

        // 전체 인원 표시
        TotalPeople.text = players.Length.ToString();

        // 준비된 인원 계산
        foreach (var player in players)
        {
            if (player.CustomProperties.ContainsKey("IsReady") && (bool)player.CustomProperties["IsReady"])
            {
                readyCount++;
            }
        }

        // 준비된 인원 표시
        ReadyPeople.text = readyCount.ToString();
    }
    public void SetPlayerReady()
    {

        Photon.Realtime.Player localPlayer = PhotonNetwork.LocalPlayer;
        localPlayer.SetCustomProperties(new Hashtable { { "IsReady", true } });
    }

}
