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
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            InitializePlayer(PhotonNetwork.LocalPlayer.ToString());
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} entered the room.");
/*        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("InitializePlayer", RpcTarget.AllBuffered, newPlayer.NickName);
        }*/
    }

    [PunRPC]
    private void InitializePlayer(string playerName)
    {
        Debug.Log("Initializing Player.");
        Vector3 spawnPoint = GetRandomSpawnPoint();

        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
        string characterPrefab = characterIndex <= 0 ? $"Player {PlayerSelection.Instance.SelectedCharacterIndex}" : $"Player {characterIndex}";

        GameObject playerObject = PhotonNetwork.Instantiate(characterPrefab, spawnPoint, Quaternion.identity);
        Debug.Log(playerObject.name);

        // 모든 플레이어에게 닉네임 표시
        PlayerCanvasAbility playerCanvasAbility = playerObject.GetComponent<PlayerCanvasAbility>();
        if (playerCanvasAbility != null)
        {
            playerCanvasAbility.SetNickname(playerName);
        }

        players[playerName] = playerObject;
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
}
