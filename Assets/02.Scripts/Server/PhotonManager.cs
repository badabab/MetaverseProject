using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    string _nickname;

    public static PhotonManager Instance;

    public GameObject StartButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartButton.gameObject.SetActive(false);
        _nickname = PlayerPrefs.GetString("LoggedInId", "Player");

        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.NickName = _nickname;

        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
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
        StartButton.gameObject.SetActive(true);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 성공!");
        Debug.Log($"RoomName: {PhotonNetwork.CurrentRoom.Name}");

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 성공!");
        Debug.Log($"RoomPlayerCount: {PhotonNetwork.CurrentRoom.PlayerCount}");

        SceneManager.LoadScene("VillageScene");

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

    private void LoadLoadingScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
