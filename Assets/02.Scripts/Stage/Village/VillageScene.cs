using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class VillageScene : MonoBehaviourPunCallbacks
{
    public static VillageScene Instance { get; private set; }
    public List<Transform> SpawnPoints;

    private bool localPlayerInitialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    private void InitializePlayer(Photon.Realtime.Player player)
    {
        if (!player.IsLocal) return;
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
}
