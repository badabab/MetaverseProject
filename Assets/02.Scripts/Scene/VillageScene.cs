using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageScene : MonoBehaviourPunCallbacks
{
    public static VillageScene Instance { get; private set; }
    public List<Transform> SpawnPoints;

    private bool isInitialized = false;
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PhotonNetwork.AutomaticallySyncScene = true; // 씬 자동 동기화 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeIfNeeded();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 마스터 클라이언트가 새로 입장한 플레이어에게 현재 씬 상태와 플레이어 정보를 전송
            photonView.RPC("SendSceneAndPlayerInfo", newPlayer);
        }

        // 새로운 플레이어에게 현재 방의 플레이어 정보 전송
        SendPlayerInfo(newPlayer);
    }

    [PunRPC]
    private void SendSceneAndPlayerInfo()
    {
        // 씬 상태 전송
        string sceneName = SceneManager.GetActiveScene().name;
        photonView.RPC("ReceiveSceneState", RpcTarget.Others, sceneName);

        // 현재 방의 모든 플레이어 정보 전송
        foreach (var player in players.Values)
        {
            photonView.RPC("ReceivePlayerInfo", RpcTarget.Others, player.GetPhotonView().ViewID);
        }
    }

    private void InitializeIfNeeded()
    {
        if (!isInitialized)
        {
            Init();
        }
    }

    private void Init()
    {
        Debug.Log("Initializing Player.");
        isInitialized = true;
        Vector3 spawnPoint = GetRandomSpawnPoint();

        string nickname = PlayerPrefs.GetString("LoggedInId");
        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
        string uniquePlayerKey = $"{nickname}_{characterIndex}";

        if (!players.ContainsKey(uniquePlayerKey))
        {
            string characterPrefab = characterIndex <= 0 ? $"Player {PlayerSelection.Instance.SelectedCharacterIndex}" : $"Player {characterIndex}";
            GameObject playerObject = PhotonNetwork.Instantiate(characterPrefab, spawnPoint, Quaternion.identity);

            players.Add(uniquePlayerKey, playerObject);

            PlayerCanvasAbility.Instance.NicknameTextUI.text = nickname;
            PlayerCanvasAbility.Instance.ShowMyNickname();
        }
        else
        {
            // 이미 플레이어가 존재하면 위치를 업데이트
            GameObject existingPlayer = players[uniquePlayerKey];
            existingPlayer.transform.position = spawnPoint;

            // 만약 기존 플레이어 오브젝트가 비활성화되어 있다면 활성화
            if (!existingPlayer.activeSelf)
            {
                existingPlayer.SetActive(true);
            }

            PlayerCanvasAbility.Instance.NicknameTextUI.text = nickname;
            PlayerCanvasAbility.Instance.ShowMyNickname();
        }
    }

    private void SendPlayerInfo(Photon.Realtime.Player newPlayer)
    {
        foreach (var player in players.Values)
        {
            photonView.RPC("ReceivePlayerInfo", newPlayer, player.GetPhotonView().ViewID);
        }
    }

    [PunRPC]
    private void ReceivePlayerInfo(int viewID)
    {
        GameObject player = PhotonView.Find(viewID).gameObject;
        if (player != null)
        {
            player.SetActive(true); // 필요한 경우 추가 작업 수행
        }
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }

    [PunRPC]
    private void RequestSceneState(Player newPlayer)
    {
        SendSceneState(newPlayer);
    }

    private void SendSceneState(Photon.Realtime.Player newPlayer)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        photonView.RPC("ReceiveSceneState", newPlayer, sceneName);
    }

    [PunRPC]
    private void ReceiveSceneState(string sceneName)
    {
        Debug.Log("Received scene state: " + sceneName);

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Init();
        }
    }
}
