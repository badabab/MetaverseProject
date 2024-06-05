using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    string _nickname;

    private void Start()
    {
        _nickname = PlayerPrefs.GetString("LoggedInId", "Player");

        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.NickName = _nickname;

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.SendRate = 50;
        PhotonNetwork.SerializationRate = 30;
    }

    public override void OnConnected()
    {
        Debug.Log("네임 서버 접속");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 접속");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 성공!");
        Debug.Log($"RoomPlayerCount: {PhotonNetwork.CurrentRoom.PlayerCount}");

        RefreshPlayerList(); // 새로운 플레이어가 방에 참여할 때마다 플레이어 목록 새로고침

        PhotonNetwork.LoadLevel("LoadingScene");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("랜덤방 입장 실패");
        Debug.Log(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 입장 실패");
        Debug.Log(message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 생성 실패");
        Debug.Log(message);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"새로운 플레이어 {newPlayer.NickName}가 방에 참여했습니다.");
        RefreshPlayerList(); // 새로운 플레이어가 방에 참여할 때마다 플레이어 목록 새로고침
    }

    void RefreshPlayerList()
    {
        Debug.Log("플레이어 목록:");
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log($"{player.NickName}");
        }
    }
}
