using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageScene : MonoBehaviourPunCallbacks
{
    public static VillageScene Instance { get; private set; }
    public List<Transform> SpawnPoints;

    private bool _init = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (!_init)
        {
            Init();
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 마스터 클라이언트는 씬을 로드
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
        }
        else
        {
            // 새로운 플레이어가 입장했을 때 마스터 클라이언트에게 씬 상태를 요청
            photonView.RPC("RequestSceneState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
        }
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("빌리지 방에 입장했습니다.");
        if (!_init)
        {
            Init();
        }        
    }

    private void Init()
    {
        Debug.Log("플레이어 초기화");
        _init = true;
        Vector3 spawnPoint = GetRandomSpawnPoint();

        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();

        if (characterIndex <= 0)
        {
            if (UI_Lobby.SelectedType == PlayerType.Female)
            {
                PhotonNetwork.Instantiate($"Player {PlayerSelection.Instance.SelectedCharacterIndex}", spawnPoint, Quaternion.identity);
                string nickname = PlayerPrefs.GetString("LoggedInId");
                PlayerCanvasAbility.Instance.ShowMyNickname();
            }
            if (UI_Lobby.SelectedType == PlayerType.Male)
            {
                PhotonNetwork.Instantiate($"Player {PlayerSelection.Instance.SelectedCharacterIndex}", spawnPoint, Quaternion.identity);
                string nickname = PlayerPrefs.GetString("LoggedInId");
                PlayerCanvasAbility.Instance.ShowMyNickname();
            }
        }
        else
        {
            PhotonNetwork.Instantiate($"Player {characterIndex}", spawnPoint, Quaternion.identity);
            string nickname = PlayerPrefs.GetString("LoggedInId");
            PlayerCanvasAbility.Instance.NicknameTextUI.text = nickname;
            PlayerCanvasAbility.Instance.ShowMyNickname();
        }
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }

    [PunRPC]
    void RequestSceneState(Photon.Realtime.Player newPlayer)
    {
        // 마스터 클라이언트가 새로운 플레이어에게 씬 상태를 전송
        SendSceneState(newPlayer);
    }

    private void SendSceneState(Photon.Realtime.Player newPlayer)
    {
        // 현재 씬의 상태를 가져옵니다 (예: 씬 이름)
        string sceneName = SceneManager.GetActiveScene().name;

        // 씬 상태를 새로운 플레이어에게 전송
        photonView.RPC("ReceiveSceneState", newPlayer, sceneName);
    }

    [PunRPC]
    void ReceiveSceneState(string sceneName)
    {
        // 씬 상태를 수신하여 처리합니다
        Debug.Log("Received scene state: " + sceneName);

        // 씬 상태를 반영합니다 (필요한 로직을 추가)
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // 현재 씬이 이미 동일하면 플레이어 스폰
            Init();
        }
    }
}
