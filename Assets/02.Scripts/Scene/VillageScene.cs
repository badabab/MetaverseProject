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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializePlayer(PhotonNetwork.LocalPlayer.ToString());
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} entered the room.");
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("InitializePlayer", RpcTarget.AllBuffered, newPlayer.NickName);
        }
    }

    [PunRPC]
    private void InitializePlayer(string playerName)
    {
        Debug.Log("플레이어 초기화");
        Vector3 spawnPoint = GetRandomSpawnPoint();

        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();

        if (characterIndex <= 0)
        {
            characterIndex = PlayerSelection.Instance.SelectedCharacterIndex;
        }

        // Instantiate player
        PhotonNetwork.Instantiate($"Player {characterIndex}", spawnPoint, Quaternion.identity);

        Debug.Log($"{playerName}");

        PlayerCanvasAbility.Instance.SetNickname(playerName);
        PlayerCanvasAbility.Instance.ShowMyNickname();
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
}
