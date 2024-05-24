using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

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
    public override void OnJoinedRoom()
    {
        Debug.Log("방에 입장했습니다.");
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
        Debug.Log($"스폰 위치: {spawnPoint}");
        if (UI_Lobby.SelectedType == PlayerType.Female)
        {
            PhotonNetwork.Instantiate($"Player {Random.Range(1, 13)}", spawnPoint, Quaternion.identity);
        }
        if (UI_Lobby.SelectedType == PlayerType.Male)
        {
            PhotonNetwork.Instantiate($"Player {Random.Range(13, 26)}", spawnPoint, Quaternion.identity);
        }       
    }
    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }
}
