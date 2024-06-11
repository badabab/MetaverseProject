using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class VillageScene : MonoBehaviourPunCallbacks
{
    public static VillageScene Instance { get; private set; }
    public List<Transform> SpawnPoints;

    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    private bool localPlayerInitialized = false;

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
        if (PhotonNetwork.InRoom && !localPlayerInitialized)
        {
            InitializePlayer(PhotonNetwork.LocalPlayer);
        }
    }

/*    public override void OnJoinedRoom()
    {
        if (!localPlayerInitialized)
        {
            InitializePlayer(PhotonNetwork.LocalPlayer);
        }
    }
*/
    private void InitializePlayer(Photon.Realtime.Player player)
    {
        if (!player.IsLocal) return;
        //Player localPlayer = FindLocalPlayer();


        Vector3 spawnPoint = GetRandomSpawnPoint();

        int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
        string characterPrefab = characterIndex <= 0 ? $"Player {PlayerSelection.Instance.SelectedCharacterIndex}" : $"Player {characterIndex}";

        PhotonNetwork.Instantiate(characterPrefab, spawnPoint, Quaternion.identity);

        localPlayerInitialized = true; // 로컬 플레이어가 생성되었음을 표시
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
    private Player FindLocalPlayer()
    {
        foreach (var player in FindObjectsOfType<Player>())
        {
            if (player.photonView.IsMine)
            {
                return player;
            }
        }
        return null;
    }
}
