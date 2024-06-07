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
            // 씬 자동 동기화가 설정되어 있으므로 LoadLevel을 사용하지 않습니다.
        }
        else
        {
            // 새로운 플레이어가 입장했을 때 마스터 클라이언트에게 씬 상태를 요청
            if (photonView != null && PhotonNetwork.LocalPlayer != null)
            {
                photonView.RPC("RequestSceneState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
            }
            else
            {
                Debug.LogWarning("PhotonView or LocalPlayer is null.");
            }
        }

        // 새로운 플레이어에게 현재 방의 플레이어 정보 전송
        SendPlayerInfo(newPlayer);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Entered Village Room.");
        InitializeIfNeeded();

        // 기존 플레이어들에게 자신을 알림
        photonView.RPC("SendPlayerInfoToAll", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
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

    [PunRPC]
    private void SendPlayerInfoToAll(Player newPlayer)
    {
        string nickname = PlayerPrefs.GetString("LoggedInId");
        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
        string uniquePlayerKey = $"{nickname}_{characterIndex}";

        if (!players.ContainsKey(uniquePlayerKey))
        {
            // 플레이어가 없는 경우 새로 생성
            Vector3 spawnPoint = GetRandomSpawnPoint();
            string characterPrefab = characterIndex <= 0 ? $"Player {PlayerSelection.Instance.SelectedCharacterIndex}" : $"Player {characterIndex}";
            GameObject playerObject = PhotonNetwork.Instantiate(characterPrefab, spawnPoint, Quaternion.identity);

            players.Add(uniquePlayerKey, playerObject);

            PlayerCanvasAbility.Instance.NicknameTextUI.text = nickname;
            PlayerCanvasAbility.Instance.ShowMyNickname();
        }
        else
        {
            // 플레이어가 이미 존재하는 경우
            GameObject existingPlayer = players[uniquePlayerKey];
            existingPlayer.transform.position = GetRandomSpawnPoint();
            existingPlayer.SetActive(true);
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
    private void RequestSceneState(Photon.Realtime.Player newPlayer)
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
