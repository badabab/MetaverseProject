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

    public override void OnJoinedRoom()
    {
        Debug.Log("Entered Village Room.");
        InitializeIfNeeded();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} entered the room.");
        InitializeIfNeeded();
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

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
}
