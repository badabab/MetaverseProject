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

/*    public override void OnJoinedRoom()
    {
        Debug.Log("Entered Village Room.");
        InitializeIfNeeded();
    }*/
/*
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} entered the room.");
        InitializeIfNeeded();
    }*/

    private void InitializeIfNeeded()
    {
       Init();
    }

    private void Init()
    {
        Debug.Log("Initializing Player.");
        isInitialized = true;
        Vector3 spawnPoint = GetRandomSpawnPoint();

        string nickname = PlayerPrefs.GetString("LoggedInId");
        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
        string uniquePlayerKey = $"{nickname}_{characterIndex}";
        string characterPrefab = characterIndex <= 0 ? $"Player {PlayerSelection.Instance.SelectedCharacterIndex}" : $"Player {characterIndex}";

        Debug.Log(uniquePlayerKey);
        Debug.Log(characterPrefab);

        GameObject playerObject = PhotonNetwork.Instantiate(characterPrefab, spawnPoint, Quaternion.identity);
        Debug.Log(playerObject.name);

        players.Add(uniquePlayerKey, playerObject);

        PlayerCanvasAbility playerCanvasAbility = playerObject.GetComponentInChildren<PlayerCanvasAbility>();
        if (playerCanvasAbility != null)
        {
            //playerCanvasAbility.SetNickname(nickname);
            playerCanvasAbility.ShowMyNickname();
        }
        else
        {
            Debug.LogError("PlayerCanvasAbility component not found on instantiated player.");
        }
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
}
