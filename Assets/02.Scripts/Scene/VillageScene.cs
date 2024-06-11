using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class VillageScene : MonoBehaviourPunCallbacks
{
    public static VillageScene Instance { get; private set; }
    public List<Transform> SpawnPoints;

    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            InitializePlayer(PhotonNetwork.LocalPlayer);
        }
    }

    public override void OnJoinedRoom()
    {
        InitializePlayer(PhotonNetwork.LocalPlayer);
    }
/*
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} entered the room.");
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("NotifyNewPlayer", newPlayer, newPlayer.NickName);
        }
    }

    [PunRPC]
    private void NotifyNewPlayer(string playerName)
    {
        Debug.Log($"Notified of new player: {playerName}");
    }*/
    private void InitializePlayer(Photon.Realtime.Player player)
    {
        if (!player.IsLocal) return;

        Vector3 spawnPoint = GetRandomSpawnPoint();

        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
        string characterPrefab = characterIndex <= 0 ? $"Player {PlayerSelection.Instance.SelectedCharacterIndex}" : $"Player {characterIndex}";

        GameObject playerObject = PhotonNetwork.Instantiate(characterPrefab, spawnPoint, Quaternion.identity);
        Debug.Log(playerObject.name);

        PlayerCanvasAbility.Instance.ShowMyNickname();
    }
    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
}
